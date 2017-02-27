using System.Collections.Generic;

namespace CistNureApi.Model.Api
{
    public class Hour
    {
        public int Type { get; set; }
        public int Val { get; set; }
        public List<int> Teachers { get; set; }
    }
}