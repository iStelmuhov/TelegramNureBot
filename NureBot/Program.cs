using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CistNureApi;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Interfaces;
using Syn.Bot.Oscova.Languages.English;
using TelegramNureBot.Model;
using TelegramNureBot.Properties;
using TelegramNureBot.Repository;
using TelegramNureBot.Repository.EF;
using TelegramNureBot.Repository.EF.Implementations;
using TelegramNureBot.Service;
using TelegramNureBot.Service.Impl;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramNureBot.Helper;
using User = TelegramNureBot.Model.User;

namespace TelegramNureBot
{
    class Program
    {
        public static readonly TelegramBotClient Bot = new TelegramBotClient("356520093:AAGKBe8YFpR5_5WIkGfoeRbdTMuOKE2O9GQ");
        public static IUserService UserService { get; set; }
        public static ResourceManager rm { get; set; }
        public static OscovaBot RecognitionSystem { get; set; }

        static void Main(string[] args)
        {
            rm = Resources.ResourceManager;

            var me = Bot.GetMeAsync().Result;
            Console.Title = me.Username;

            Bot.OnMessage += Bot_OnMessage;
            Bot.OnReceiveError += Bot_OnReceiveError;

            RecognitionSystem = new OscovaBot();
            RecognitionSystem.MainUser.Context.SharedData.Add(Bot);
            RecognitionSystem.MainUser.Context.SharedData.Add(UserService);
            RecognitionSystem.Language.Stemmer = new RussianStemmer();
            RecognitionSystem.Language.StopWords = StopWordsGenerator.GenerateRussianStopWords();
            //TODO WORDNET

            using (var dbContext = new NureBotDbContext(true))
            {
                dbContext.Database.Initialize(true);
                UserService = new UserService(new UserRepository(dbContext, dbContext.Users));

                Bot.SetWebhookAsync();

                Bot.StartReceiving();
                Console.ReadLine();
                Bot.StopReceiving();
            }
        }

        private static void Bot_OnReceiveError(object sender, Telegram.Bot.Args.ReceiveErrorEventArgs e)
        {
            Debugger.Break();
        }

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;

            User user = CheckRegistration(message);
            CheckUserRoleAsync(user, message);

            if (user.Role == Role.NotSet) return;

            BotUser recognitionUser = RecognitionSystem.Users.FirstOrDefault(a => a.ID == user.Id.ToString()) ??
                                      RecognitionSystem.CreateUser(user.ToString());

            recognitionUser.Context.SharedData.Add(user);

            var request = recognitionUser.CreateRequest(message.Text);
            var evaluateRequest=RecognitionSystem.Evaluate(request);
            evaluateRequest.Invoke();

            await Bot.SendTextMessageAsync(message.Chat.Id, evaluateRequest.Serialize());
        }


        public static User CheckRegistration(Message message)
        {
            try
            {
                User user = UserService.View(message.Chat.Id);
                return user;
            }
            catch (Exception)
            {
                return UserService.CreateUser(message.Chat.Id, message.From.FirstName, Role.NotSet);
            }

        }
        public static async void CheckUserRoleAsync(User user, Message message)
        {
            if (user.Role == Role.NotSet)
                if (!message.Text.StartsWith(Emoji.Star))
                {
                    RequestUserRole(user, message.Chat.Id);
                }
                else if (message.Text.StartsWith(Emoji.Star))
                {

                    string msg = message.Text.Replace(Emoji.Star, "");

                    if (msg.Equals(rm.GetString("Student")))
                        UserService.ChangeRole(user.Id, Role.Student);
                    else if (msg.Equals(rm.GetString("Teacher")))
                        UserService.ChangeRole(user.Id, Role.Teacher);
                    else if (msg.Equals(rm.GetString("None")))
                        UserService.ChangeRole(user.Id, Role.None);

                    await Bot.SendTextMessageAsync(message.Chat.Id, rm.GetString("RoleSetMessage"),
                    replyMarkup: new ReplyKeyboardHide());
                }
        }
        public static async void RequestUserRole(User user, long chatId)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
   {
                    new []
                    {
                        new KeyboardButton(Emoji.Star + rm.GetString("Student")),
                    },
                    new []
                    {
                        new KeyboardButton(Emoji.Star + rm.GetString("Teacher")),
                    },
                    new []
                    {
                        new KeyboardButton(Emoji.Star + rm.GetString("None")),
                    }
                });

            await Bot.SendTextMessageAsync(chatId,
                $"{rm.GetString("NewUserMessage")} {user.FirstName},{rm.GetString("ChooseRole")}", replyMarkup: keyboard);

        }
    }

}
