using System;
using System.Linq;
using CistNureApi;
using CistNureApi.Model.Dto;
using Model;
using OpenWeatherMap;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using Syn.Bot.Oscova.Entities;
using TelegramNureBot.Helper;

namespace TelegramNureBot.Dialogs
{
    public class WeatherDialog:Dialog
    {
        [Expression("Погода")]
        [Expression("Холодно ли на улице")]
        public async void ShowTodayWeather(Context context, Result result)
        {

            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            MessageTransfer msg = new MessageTransfer() { ChatId = user.Id };

            var openClient=new OpenWeatherMapClient("22a6ae75fa60f187c896025ec05038ce");
            var currentWeather = await openClient.CurrentWeather.GetByCityId(706483,MetricSystem.Metric,OpenWeatherMapLanguage.RU);

            msg.Message =$"В Харькове сейчас {currentWeather.Temperature.Value}° {Emoji.Sunny}\n" +
                         $"На небе {currentWeather.Weather.Value}\n" +
                         $"Обновленно:{currentWeather.LastUpdate.Value}";
            result.SendResponse(msg.ToJson());
        }

        [Expression("Погода на {ruDate}")]
        [Expression("Погода в {ruDate}")]
        [Expression("Погода {ruDate}")]
        [Entity(Sys.Date)]
        public async void ShowWeather(Context context, Result result)
        {
            var user = context.SharedData.OfType<User>();
            context.SharedData.Remove(user);
            var day = result.Entities.OfType<DateEntity>(Sys.Date);

            MessageTransfer msg = new MessageTransfer() { ChatId = user.Id };

            var openClient = new OpenWeatherMapClient("22a6ae75fa60f187c896025ec05038ce");
            var currentWeather = await openClient.Forecast.GetByCityId(706483, true, MetricSystem.Metric, OpenWeatherMapLanguage.RU);

            var dayWeather = currentWeather.Forecast.SingleOrDefault(a => a.Day.ToShortDateString().Equals(day.DateTime.ToShortDateString()));
            
            msg.Message = dayWeather == null ? $"Извините, но у мен не вышло найти прогноз на {day.DateTime.ToShortDateString()}" : $"В Харькове будет от {dayWeather.Temperature.Min}° до {dayWeather.Temperature.Max}° {Emoji.Sunny}\n";

            result.SendResponse(msg.ToJson());
        }
    }
}