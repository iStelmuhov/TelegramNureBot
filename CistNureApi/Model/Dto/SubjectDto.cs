using System;
using System.Collections.Generic;
using System.Linq;

namespace CistNureApi.Model.Dto
{
    public class SubjectDto
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberPair { get; set; }
        public string Type { get; set; }
        public string Auditory { get; set; }
        public List<string> Teachers { get; set; }

        public SubjectDto()
        {
            Teachers=new List<string>();
        }

        public override string ToString()
        {
            string result= $"Название:{Name} {Type}\nНомер пары: {NumberPair}  Время:{StartTime:HH:mm}-{EndTime:HH:mm}\nАудитория:{Auditory}\n";

            result += "Преподователи:";
            foreach (var teacher in Teachers)

            {
                result += teacher+", ";
            }

            
            return result.Substring(0,result.LastIndexOf(", ", StringComparison.Ordinal));
        }
    }
}