using System.Collections.Generic;
using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class Event
    {
        [JsonProperty("subject_id")]
        public int SubjectId { get; set; }
        [JsonProperty("start_time")]
        public int StartTime { get; set; }
        [JsonProperty("end_time")]
        public int EndTime { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("number_pair")]
        public int NumberPair { get; set; }
        public string Auditory { get; set; }
        public List<int> Teachers { get; set; }
        public List<int> Groups { get; set; }
    }
}