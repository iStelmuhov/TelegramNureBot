using System.Collections.Generic;
using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class Department
    {
        public int Id { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}