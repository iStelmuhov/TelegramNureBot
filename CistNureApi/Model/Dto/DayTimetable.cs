using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using CistNureApi.Model.Api;

namespace CistNureApi.Model.Dto
{
    public class DayTimetable
    {
        public DateTime Date { get; set; }
        private List<SubjectDto> Subjects { get; set; }

        public DayTimetable()
        {
            Subjects = new List<SubjectDto>();
        }

        public DayTimetable(DateTime date, List<SubjectDto> subjects)
        {
            Date = date;
            Subjects = subjects;
        }

        public DayTimetable(RootEventObject eventObject, DateTime date)
        {
            if (eventObject == null)
                throw new ArgumentNullException(nameof(eventObject));

            Subjects = new List<SubjectDto>();
            Date = date;
            foreach (var eventObj in eventObject.Events)
            {
                SubjectDto subject = new SubjectDto();
                subject.Name = eventObject.Subjects.FirstOrDefault(a => a.Id == eventObj.SubjectId).Brief;
                subject.StartTime = UnixTimeStampToDateTime(eventObj.StartTime);
                subject.EndTime = UnixTimeStampToDateTime(eventObj.EndTime);
                subject.NumberPair = eventObj.NumberPair;
                subject.Auditory = eventObj.Auditory;
                subject.Type = eventObject.types.FirstOrDefault(a => a.Id == eventObj.Type).ShortName;

                foreach (var teacher in eventObj.Teachers)
                {
                    subject.Teachers.Add(eventObject.Teachers.FirstOrDefault(a => a.Id == teacher).ShortName);
                }

                Subjects.Add(subject);
            }
        }

        public static DayTimetable GetNearestTimeTable(RootEventObject eventObject)
        {
            

            var first = eventObject.Events.ElementAt(0);
            if (first == null) return null;

            DayTimetable time = new DayTimetable {Date = UnixTimeStampToDateTime(first.StartTime)};

            SubjectDto subject = new SubjectDto
            {
                Name = eventObject.Subjects.FirstOrDefault(a => a.Id == first.SubjectId).Brief,
                StartTime = UnixTimeStampToDateTime(first.StartTime),
                EndTime = UnixTimeStampToDateTime(first.EndTime),
                NumberPair = first.NumberPair,
                Auditory = first.Auditory,
                Type = eventObject.types.FirstOrDefault(a => a.Id == first.Type).ShortName
            };
            foreach (var teacher in first.Teachers)
            {
                subject.Teachers.Add(eventObject.Teachers.FirstOrDefault(a => a.Id == teacher).ShortName);
            }
            time.Subjects.Add(subject);

            return time;
        }
        public override string ToString()
        {
            string result = $"День:{Date:dd MMMM}\n";
            foreach (var subjectDto in Subjects)
            {
                result += subjectDto.ToString() + "\n\n";
            }

            return result;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}