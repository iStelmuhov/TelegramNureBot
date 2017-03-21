using System;
using CistNureApi;
using CistNureApi.Model.Dto;
using Model;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using Syn.Bot.Oscova.Entities;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramNureBot.WPF.Helper;
using User = Model.User;


namespace TelegramNureBot.WPF.Dialogs
{
    public class CistDialog : Dialog
    {
        [Expression("Покажи мне моё расписание")]
        [Expression("Покажи мне расписание")]
        [Expression("На сколько мне?")]
        [Expression("На когда у меня пары?")]
        [Expression("Когда мне нужно быть в вузе?")]
        public void ShowTodayTimetable(Context context, Result result)
        {

            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            MessageTransfer msg = new MessageTransfer() { ChatId = user.Id };

            DayTimetable timetable = new DayTimetable();
            switch (user.Role)
            {
                case Role.Student:
                    timetable = CistApi.GetGroupTimetable((user as Student).GroupId, DateTime.Now);
                    break;
                case Role.Teacher:
                    timetable = CistApi.GetTeacherTimetable((user as Teacher).TeacherId, DateTime.Now);
                    break;
                default:
                    break;
            }

            msg.Message = timetable?.ToString() ?? "На сегодня пар нету!";
            result.SendResponse(msg.ToJson());
        }

        [Expression("Покажи мне моё расписание на {ruDate}")]
        [Expression("Покажи мне расписание на {ruDate}")]
        [Expression("Расписание на {ruDate}")]
        [Expression("На сколько мне {ruDate}")]
        [Expression("Пары на {ruDate}")]
        [Entity(Sys.Date)]
        public void ShowNextDayTimetable(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            MessageTransfer msg = new MessageTransfer() { ChatId = user.Id };

            var day = result.Entities.OfType<DateEntity>(Sys.Date);

            DayTimetable timetable = new DayTimetable();
            switch (user.Role)
            {
                case Role.Student:
                    timetable = CistApi.GetGroupTimetable((user as Student).GroupId, day.DateTime);
                    break;
                case Role.Teacher:
                    timetable = CistApi.GetTeacherTimetable((user as Teacher).TeacherId, day.DateTime);
                    break;
                default:
                    break;
            }

            msg.Message = timetable?.ToString() ?? $"В расписании нету пар на {day.DateTime.ToShortDateString()}";
            result.SendResponse(msg.ToJson());
        }

        [Expression("Расписание @groupName")]
        [Expression("Расписание @groupName на {ruDate}")]
        [Expression("Покажи мне расписание @groupName на {ruDate}")]
        [Expression("Пары @groupName на {ruDate}")]
        [Entity(Sys.Date)]
        public void ShowGroupTimetable(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            MessageTransfer msg = new MessageTransfer() { ChatId = user.Id };

            var group = result.Entities.OfType("groupName");
            var day = result.Entities.OfType<DateEntity>(Sys.Date) ?? new DateEntity("", DateTime.Now);

            int groupId = -1;
            try
            {
                groupId = CistApi.GetGroupIdFromName(group.Value);
            }
            catch (Exception)
            {
                msg.Message = "Что-то я не нашел такой группы, попробуй ещё раз!";
                result.SendResponse(msg.ToJson());
                return;
            }

            DayTimetable timetable = CistApi.GetGroupTimetable(groupId, day.DateTime);

            msg.Message = timetable?.ToString() ?? $"В расписании нету пар на {day.DateTime.ToShortDateString()}";
            result.SendResponse(msg.ToJson());
        }

        [Expression("Расписание @teacherName на {ruDate}")]
        [Expression("Покажи мне расписание @teacherName на {ruDate}")]
        [Expression("Пары @teacherName на {ruDate}")]
        [Entity(Sys.Date)]
        public void ShowTeacherTimetable(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            MessageTransfer msg = new MessageTransfer() { ChatId = user.Id };

            var teacher = result.Entities.OfType("teacherName");
            var day = result.Entities.OfType<DateEntity>(Sys.Date);
            int teacherId = -1;
            try
            {
                teacherId = CistApi.GetTeacherIdFromName(teacher.Value);
            }
            catch (Exception)
            {
                msg.Message = "Что-то я не нашел такого преподователя, попробуй ещё раз!";
                result.SendResponse(msg.ToJson());
                return;
            }

            DayTimetable timetable = CistApi.GetTeacherTimetable(teacherId, day.DateTime);

            msg.Message = timetable?.ToString() ?? $"В расписании нету пар на {day.DateTime.ToShortDateString()}";
            result.SendResponse(msg.ToJson());
        }

        [Expression("Когда будет @teacherName")]
        [Expression("Ближайщая пара @teacherName")]
        public void ShowTeacherNearestTimetable(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            var msg = new MessageTransfer() { ChatId = user.Id };

            var teacher = result.Entities.OfType("teacherName");
            var day = DateTime.Now;
            int teacherId = -1;
            try
            {
                teacherId = CistApi.GetTeacherIdFromName(teacher.Value);
            }
            catch (Exception)
            {
                msg.Message = "Что-то я не нашел такого преподователя, попробуй ещё раз!";
                result.SendResponse(msg.ToJson());
                return;
            }

            var timetable = CistApi.GetTeacherNearestTimetable(teacherId, day);

            msg.Message = timetable != null
                ? $"Ближайшая пара у {teacher}:\n" + timetable?.ToString()
                : $"В расписании нету пар для {teacher}";

            result.SendResponse(msg.ToJson());
        }

        [Expression("Новости")]
        [Expression("Новости ВУЗа")]
        public void ShowNureFeed(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            var msg = new MessageTransfer() { ChatId = user.Id };


            try
            {
                msg.Message = CistApi.GetNureFeed();
            }
            catch (Exception)
            {
                msg.Message = "К сожалению, сервер с новостями не работает!";
                result.SendResponse(msg.ToJson());
                return;
            }

            result.SendResponse(msg.ToJson());
        }

        [Expression("Дорога до университета")]
        [Expression("Как доехать")]
        [Expression("Как доехать до университета")]
        [Expression("Местоположение")]
        [Expression("Время до университета")]
        [Expression("Маршрут до университета")]
        [Expression("Маршрут")]
        public void ShowMap(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            var msg = new MessageTransfer() { ChatId = user.Id };

            msg.Message = "Отправьте пожалуйста своё местоположение!";
            msg.ReplyMarkup = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton($"{Emoji.Round_Pushpin} Моё местоположение")
                            {
                                RequestLocation = true
                            }
                        }
                    );

            result.SendResponse(msg.ToJson());
        }

        [Expression]
        public void Default(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            MessageTransfer msg = new MessageTransfer
            {
                ChatId = user.Id,
                Message = $"Ой, что-то я не разобрал что тут написано {Emoji.Confused} \nМожете повторить ввод?"
            };
            result.SendResponse(msg.ToJson());
        }

    }
}