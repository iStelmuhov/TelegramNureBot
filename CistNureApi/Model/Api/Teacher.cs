using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class Teacher
    {
        public int Id { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }
}