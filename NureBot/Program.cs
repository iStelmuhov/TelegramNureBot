using System;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using Microsoft.Practices.Unity;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CistNureApi;
using Dependencies;
using Model;
using Newtonsoft.Json;
using Repository.EF;
using Service;
using Syn.Bot.Oscova;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramNureBot.Dialogs;
using TelegramNureBot.Helper;
using TelegramNureBot.Properties;
using User = Model.User;
using Teacher = Model.Teacher;

namespace TelegramNureBot
{
    class Program
    {
        [Dependency]
        public static TelegramBotClient Bot { get; set; }
        [Dependency]
        public static IUserService UService { get; set; }
        [Dependency]
        public static OscovaBot RecognitionSystem { get; set; }

        public static ResourceManager Rm => Resources.ResourceManager;

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

            var dbContext = new NureBotDbContext(false);
            var unityContainer = new UnityContainer();
            dbContext.Database.Initialize(true);
            ContainerBoostraper.RegisterTypes(unityContainer, dbContext, "356520093:AAGKBe8YFpR5_5WIkGfoeRbdTMuOKE2O9GQ");

            Bot = unityContainer.Resolve<TelegramBotClient>();
            UService = unityContainer.Resolve<IUserService>();
            RecognitionSystem = unityContainer.Resolve<OscovaBot>();

            Console.Title = Bot.GetMeAsync().Result.Username;

            Bot.OnMessage += Bot_OnMessage;
            Bot.OnReceiveError += Bot_OnReceiveError;

            RecognitionSystem.Dialogs.Add(new CistDialog());
            RecognitionSystem.MainUser.Context.SharedData.Add(Bot);
            RecognitionSystem.MainUser.Context.SharedData.Add(UService);
            RecognitionSystem.Language.Stemmer = new RussianStemmer();
            RecognitionSystem.Language.StopWords = StopWordsGenerator.GenerateRussianStopWords();

            RecognitionSystem.Trainer.StartTraining();
            RecognitionSystem.Recognizers.Clear();
            RecognitionSystem.Recognizers.Add(new RuDateRecognizer());
            RecognitionSystem.Language.Culture.DateTimeFormat =
                CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
            RecognitionSystem.Recognizers.Add(new GroupRecognizer());
            RecognitionSystem.CreateRecognizer("teacherName", new Regex(@"[А-Яа-я]+ [А-Яа-я]\.? [А-Яа-я]\.?"));
            //TODO WORDNET

            RecognitionSystem.MainUser.ResponseReceived += async (sender, arg) =>
            {
                var msg = JsonConvert.DeserializeObject<MessageTransfer>(arg.Response.Text);

                await Bot.SendTextMessageAsync(msg.ChatId, msg.Message);
            };


            Bot.SetWebhookAsync();
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();

            dbContext.Dispose();
            unityContainer.Dispose();
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
            user = await CheckUserRoleAsync(user, message);

            if (user.Role == Role.NotSet) return;

            if (user.Role == Role.Student)
            {
                var student = user as Student;
                if (student == null) return;

                switch (student.GroupId)
                {
                    case 0:
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Будь добр, напиши из какой ты группы?");
                        UService.ChangeGroup(user.Id, -1);
                        return;
                    case -1:
                        try
                        {
                            int id = CistApi.GetGroupIdFromName(message.Text);
                            UService.ChangeGroup(user.Id, id);
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Отлично, я запомнил это!");

                        }
                        catch (Exception)
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Что-то у меня не получается найти такую группу, попробуй ещё раз!");
                            return;
                        }
                        return;
                    default: break;
                }

            }
            else if (user.Role == Role.Teacher)
            {
                var teacher = user as Teacher;
                if (teacher == null) return;

                switch (teacher.TeacherId)
                {
                    case 0:
                        await Bot.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, напишите ваше ФИО");
                        UService.ChangeTeacherId(teacher.Id, -1);
                        return;
                    case -1:
                        try
                        {
                            int id = CistApi.GetTeacherIdFromName(message.Text);
                            UService.ChangeTeacherId(user.Id, id);
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Отлично, я запомнил это!");
                            return;
                        }
                        catch (Exception)
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Что-то у меня не получается найти вас, попробуйте еще раз!");
                        }
                        return;
                    default: break;
                }
            }


            RecognitionSystem.MainUser.Context.SharedData.Add(user);

            var evaluateRequest = RecognitionSystem.Evaluate(message.Text);
            try
            {
                evaluateRequest.Invoke();
            }
            catch (Exception)
            {
                await Bot.SendTextMessageAsync(message.Chat.Id,
                    $"Извини, не получилось выполнить твой запроc{Emoji.Disappointed}\n" + 
                    $"Какие-то проблемы на сервере\n" + 
                    $"Попробуй по позже!");
            }
            
        }


        public static User CheckRegistration(Message message)
        {
            try
            {
                var user = UService.View<User>(message.Chat.Id);
                switch (user.Role)
                {
                    case Role.Student:
                        user = UService.View<Student>(message.Chat.Id);
                        break;
                    case Role.Teacher:
                        user = UService.View<Teacher>(message.Chat.Id);
                        break;
                    default:
                        break;
                }
                return user;
            }
            catch (Exception)
            {
                return UService.CreateUser(message.Chat.Id, message.From.FirstName, Role.NotSet);
            }

        }
        public static async Task<User> CheckUserRoleAsync(User user, Message message)
        {
            if (user.Role == Role.NotSet)
                if (!message.Text.StartsWith(Emoji.Star))
                {
                    RequestUserRole(user, message.Chat.Id);
                }
                else if (message.Text.StartsWith(Emoji.Star))
                {

                    string msg = message.Text.Replace(Emoji.Star, "");

                    if (msg.Equals(Rm.GetString("Student")))
                    {
                        UService.ChangeRole(user.Id, Role.Student);
                        user = UService.View<Student>(user.Id);
                    }
                    else if (msg.Equals(Rm.GetString("Teacher")))
                    {
                        UService.ChangeRole(user.Id, Role.Teacher);
                        user = UService.View<Teacher>(user.Id);
                    }

                    await Bot.SendTextMessageAsync(message.Chat.Id, Rm.GetString("RoleSetMessage"),
                    replyMarkup: new ReplyKeyboardHide());
                }
            return user;
        }
        public static async void RequestUserRole(User user, long chatId)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
   {
                    new []
                    {
                        new KeyboardButton(Emoji.Star + Rm.GetString("Student")),
                    },
                    new []
                    {
                        new KeyboardButton(Emoji.Star + Rm.GetString("Teacher")),
                    }
                });

            await Bot.SendTextMessageAsync(chatId,
                $"{Rm.GetString("NewUserMessage")} {user.FirstName},{Rm.GetString("ChooseRole")}", replyMarkup: keyboard);

        }
    }

}
