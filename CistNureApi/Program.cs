using System;

namespace CistNureApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var a = CistApi.ReciveAllTeachers();
            Console.WriteLine(a.ToString());
        }
    }
}