using System.Collections.Generic;
using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class University
    {
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
}