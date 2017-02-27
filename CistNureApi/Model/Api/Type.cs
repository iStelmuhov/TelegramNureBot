using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class Type
    {
        public int Id { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("id_base")]
        public int IdBase { get; set; }
        [JsonProperty("type")]
        public string SubjectType { get; set; }
    }
}