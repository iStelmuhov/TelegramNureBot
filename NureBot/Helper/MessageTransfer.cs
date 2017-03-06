using Newtonsoft.Json;

namespace TelegramNureBot.Helper
{
    public class MessageTransfer
    {
        public long ChatId { get; set; }
        public string Message { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}