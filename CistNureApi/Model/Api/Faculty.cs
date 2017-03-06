using System.Collections.Generic;
using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class Faculty
    {
        public int Id { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        public List<Direction> Directions { get; set; }
        public List<Department> Departments { get; set; }
    }
}