using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CistNureApi.Model.Api;
using CistNureApi.Model.Dto;
using Newtonsoft.Json;
using Yandex.Translator;

namespace CistNureApi
{
    public static class CistApi
    {
        private const string ApiRoot = "http://cist.nure.ua/ias/app/tt/";

        private const string YandexTranslatorApiKey =
            "trnsl.1.1.20170225T144453Z.2fd40b5a30817375.225f419f3a90f8d2ea82695912a596caa4d275e9";

        private static readonly IYandexTranslator Translator;
        static CistApi()
        {
            Translator =
                 Yandex.Translator.Yandex.Translator(api => api.ApiKey(YandexTranslatorApiKey).Format(ApiDataFormat.Json));
        }
        public static IList<Group> ReceiveAllGroups()
        {
            string json;
            List<Group> groups = new List<Group>();

            using (var webClient = new WebClient())
            {
                json = webClient.DownloadString(new Uri(ApiRoot + "P_API_GROUP_JSON"));
            }

            var root = JsonConvert.DeserializeObject<RootUniversityObject>(json);

            foreach (var universityFaculty in root.University.Faculties)
            {
                if (universityFaculty != null)
                    foreach (var direction in universityFaculty.Directions)
                    {
                        if (direction.Groups != null)
                            groups.AddRange(direction.Groups);

                        if (direction.Specialities != null)

                            foreach (var spec in direction.Specialities)
                            {
                                if (spec.Groups != null)
                                    groups.AddRange(spec.Groups);
                            }

                    }
            }

            return groups;
        }
        public static IList<Teacher> ReciveAllTeachers()
        {
            string json;
            List<Teacher> teachers = new List<Teacher>();

            using (WebClient webClient = new WebClient())
            {
                json = webClient.DownloadString(new Uri(ApiRoot + "P_API_PODR_JSON"));
            }

            var root = JsonConvert.DeserializeObject<RootUniversityObject>(json);

            foreach (var universityFaculty in root.University.Faculties)
            {
                if (universityFaculty != null)
                    teachers.AddRange(universityFaculty.Departments.Where(dep => dep != null).SelectMany(dep => dep.Teachers));
            }

            return teachers.Distinct().ToList();
        }

        public static int GetGroupIdFromName(string name)
        {
            var ukName = name.ToUpper().Replace('И', 'І');
            var group = ReceiveAllGroups().FirstOrDefault(a => a.Name.Equals(ukName, StringComparison.OrdinalIgnoreCase));


            if (group == null) throw new ArgumentOutOfRangeException();

            return group.Id;
        }

        public static string GetGroupNameFromId(int id)
        {

            var group = ReceiveAllGroups().FirstOrDefault(a => a.Id == id);

            if (group == null) throw new ArgumentOutOfRangeException();

            return group.Name;

        }

        public static DayTimetable GetGroupTimetable(int id, DateTime from)
        {
            string json;

            using (WebClient webClient = new WebClient())
            {
                json =
                    webClient.DownloadString(
                        new Uri(ApiRoot + "P_API_EVENTS_GROUP_JSON?" + $"p_id_group ={id}&time_from={ConvertToUnixTimestamp(from.AddDays(-1))}&time_to={ConvertToUnixTimestamp(from)}"));
            }

            RootEventObject root;
            try
            {
                root = JsonConvert.DeserializeObject<RootEventObject>(json);
            }
            catch (Exception)
            {
                return null;
            }


            return new DayTimetable(root, from);
        }

        public static int GetTeacherIdFromName(string name)
        {

            string ukName = TranslateFio(name);

            var teacher = ReciveAllTeachers().FirstOrDefault(a => a.ShortName.Split(' ')[0].Equals(ukName.Split(' ')[0], StringComparison.OrdinalIgnoreCase));

            if (teacher == null) throw new ArgumentOutOfRangeException();

            return teacher.Id;
        }
        public static string GetTeacherNameFromId(int id)
        {

            var teacher = ReceiveAllGroups().FirstOrDefault(a => a.Id == id);

            if (teacher == null) throw new ArgumentOutOfRangeException();

            return teacher.Name;

        }

        public static DayTimetable GetTeacherTimetable(int id, DateTime from)
        {
            string json;

            using (WebClient webClient = new WebClient())
            {
                json =
                    webClient.DownloadString(
                        new Uri(ApiRoot + "P_API_EVENTS_TEACHER_JSON?" + $"p_id_teacher ={id}&time_from={ConvertToUnixTimestamp(from.AddDays(-1))}&time_to={ConvertToUnixTimestamp(from)}"));
            }

            RootEventObject root;
            try
            {
                root = JsonConvert.DeserializeObject<RootEventObject>(json);
            }
            catch (Exception)
            {
                return null;
            }


            return new DayTimetable(root, from);
        }
        public static DayTimetable GetTeacherNearestTimetable(int id, DateTime from)
        {
            string json;

            using (WebClient webClient = new WebClient())
            {
                json =
                    webClient.DownloadString(
                        new Uri(ApiRoot + "P_API_EVENTS_TEACHER_JSON?" + $"p_id_teacher={id}&time_from={ConvertToUnixTimestamp(from.AddDays(-1))}"));
            }

            RootEventObject root;
            try
            {
                root = JsonConvert.DeserializeObject<RootEventObject>(json);
            }
            catch (Exception)
            {
                return null;
            }


            return DayTimetable.GetNearestTimeTable(root);
        }
        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        private static string TranslateFio(string fio)
        {
            var ukName = Translator.Translate("uk", fio).Text;
            var list = ukName.Split(new char[] { ' ' });
            return $"{list.ElementAt(0)} {list.ElementAt(1)[0]}. {list.ElementAt(2)[0]}.";
        }
    }
}
