using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using TelegramNureBot.Model;
using TelegramNureBot.Service.Impl;

namespace TelegramNureBot.Dialogs
{
    public class CistDialog:Dialog
    {
        [Expression("Покажи мне моё расписание")]
        [Expression("Покажи мне расписание")]
        [Expression("Во сколько мне сегодня?")]
        [Expression("На сколько мне сегодня?")]
        [Expression("На когда у меня пары?")]
        [Expression("Когда мне нужно быть в вузе?")]
        public void ShowTimetable(Context context, Result result)
        {
            var userService = context.SharedData.OfType<UserService>();
            var user = context.SharedData.OfType<User>();
            result.SendResponse("Done");
        }
    }
}