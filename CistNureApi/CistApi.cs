using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CistNureApi.Model;
using CistNureApi.Model.Api;
using CistNureApi.Model.Dto;
using Newtonsoft.Json;
using Yandex.Translator;
using static Yandex.Translator.Yandex;

namespace CistNureApi
{
    public static class CistApi
    {
        public const string ApiRoot = "http://cist.nure.ua/ias/app/tt/";

        private const string YandexTranslatorApiKey =
            "trnsl.1.1.20170225T144453Z.2fd40b5a30817375.225f419f3a90f8d2ea82695912a596caa4d275e9";

        public static IYandexTranslator Translator;
        static CistApi()
        {
            Translator =
                 Yandex.Translator.Yandex.Translator(api => api.ApiKey(YandexTranslatorApiKey).Format(ApiDataFormat.Json));
        }
        public static IList<Group> ReceiveAllGroups()
        {
            string json;
            List<Group> groups = new List<Group>();

            using (WebClient webClient = new WebClient())
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
                            foreach (var directionGroup in direction.Groups)
                            {
                                groups.Add(directionGroup);
                            }

                        if (direction.Specialities != null)

                            foreach (var spec in direction.Specialities)
                            {
                                if (spec.Groups != null)
                                    foreach (var specGroup in spec.Groups)
                                    {
                                        groups.Add(specGroup);
                                    }
                            }

                    }
            }

            return groups;
        }

        public static int GetGroupIdFromName(string name)
        {
            var ukName =Translator.Translate("uk", name).Text;

            return ReceiveAllGroups().FirstOrDefault(a => a.Name.Equals(ukName, StringComparison.OrdinalIgnoreCase)).Id;

        }

        public static DayTimetable GetGroupTimetable(int id, DateTime from)
        {
            string json;

            using (WebClient webClient = new WebClient())
            {
                json =
                    webClient.DownloadString(
                        new Uri(ApiRoot+ "P_API_EVENTS_GROUP_JSON?" + $"p_id_group ={id}&time_from={ConvertToUnixTimestamp(from)}&time_to={ConvertToUnixTimestamp(from.AddDays(1))}"));
            }

            var root = JsonConvert.DeserializeObject<RootEventObject>(json);

            return new DayTimetable(root, from);
        }


        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
