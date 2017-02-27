using System.Collections.Generic;

namespace CistNureApi.Model.Api
{
    public class Subject
    {
        public int Id { get; set; }
        public string Brief { get; set; }
        public string Title { get; set; }
        public List<Hour> Hours { get; set; }
    }
}