using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramNureBot.WPF.Helper
{
    public class MessageTransfer
    {
        public long ChatId { get; set; }
        public string Message { get; set; }
        public ReplyKeyboardMarkup ReplyMarkup { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}