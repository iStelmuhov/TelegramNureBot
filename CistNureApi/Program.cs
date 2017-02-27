using System;

namespace CistNureApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var a=CistApi.GetGroupTimetable(CistApi.GetGroupIdFromName("кі-13-3"),DateTime.Today.AddDays(1));
            Console.WriteLine(a.ToString());
        }
    }
}