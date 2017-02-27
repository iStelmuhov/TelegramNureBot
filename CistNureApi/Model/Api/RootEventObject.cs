using System.Collections.Generic;
using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class RootEventObject
    {
        [JsonProperty("time-zone")]
        public string TimeZone { get; set; }
        public List<Event> Events { get; set; }
        public List<Group> Groups { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Type> types { get; set; }
    }
}