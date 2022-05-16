using System;
using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class CalendarModel
    {
        public static int CreateDiaryEvent(string name, string dateStart, string dateEnd)
        {
            var diaryEvent = new CalendarDiaryEvent();
            var start = DateTime.MinValue;
            var end = DateTime.MinValue;

            if (DateTime.TryParse(dateStart, out start) && DateTime.TryParse(dateEnd, out end))
            {
                diaryEvent.Name = name;
                diaryEvent.DateStart = start;
                diaryEvent.DateEnd = end.AddDays(1);                
                if (diaryEvent.IsValid())
                {
                    return Logic.Logic.Instance.CalendarDiaryEvent.Create(diaryEvent);
                }
            }

            return 0;
        }

        internal static IEnumerable<DiaryEventJSON> LoadAllAppointmentsInDateRange(DateTime fromDate, DateTime toDate)
        {
            var result = new List<DiaryEventJSON>();

            foreach (var ev in Logic.Logic.Instance.CalendarDiaryEvent.GetInRange(fromDate, toDate))
            {
                var dejson = new DiaryEventJSON()
                {
                    ID = ev.Id,
                    Title = ev.Name,
                    SomeImportantKeyID = 1,
                    StartDateString = ev.DateStart.ToString("s"),
                    EndDateString = ev.DateEnd.ToString("s"),
                    StatusString = "ss1",
                    StatusColor = "blue",
                    ClassName = "",
                    AllDay = false,
                    StartEditable = Logic.Logic.Instance.AccountAccess.IsAdminOrBuh(),
                };

                result.Add(dejson);
            }

            var months = new int[] { fromDate.Month, fromDate.AddMonths(1).Month, fromDate.AddMonths(2).Month };

            var birthdays = Logic.Logic.Instance.Account.GetAll().Where(x => x.DateBirth.Year != 1900 && months.Contains(x.DateBirth.Month));

            foreach (var b in birthdays)
            {

                result.Add(
                    new DiaryEventJSON()
                    {
                        ID = 0,
                        Title = $"День Рождения {b.Name}",
                        SomeImportantKeyID = 1,
                        StartDateString = new DateTime(fromDate.Year, b.DateBirth.Month, b.DateBirth.Day).ToString("s"),
                        EndDateString = new DateTime(fromDate.Year, b.DateBirth.Month, b.DateBirth.Day).ToString("s"),
                        StatusColor = "red",
                        ClassName = "",
                        AllDay = false,
                        StartEditable = false,
                    });

                if (fromDate.Year != toDate.Year)
                {
                    result.Add(
                    new DiaryEventJSON()
                    {
                        ID = 0,
                        Title = $"День Рождения {b.Name}",
                        SomeImportantKeyID = 1,
                        StartDateString = new DateTime(toDate.Year, b.DateBirth.Month, b.DateBirth.Day).ToString("s"),
                        EndDateString = new DateTime(toDate.Year, b.DateBirth.Month, b.DateBirth.Day).ToString("s"),
                        StatusColor = "red",
                        ClassName = "",
                        AllDay = false,
                        StartEditable = false,
                    });
                }

            }

            return result;
        }

        internal static bool DeleteDiaryEvent(int id)
        {
            return Logic.Logic.Instance.CalendarDiaryEvent.Delete(id).Success;
        }

        internal static bool UpdateDateStart(int eventId, string dateStart, string dateEnd)
        {
            var end = DateTime.MinValue;

            DateTime start;
            if (DateTime.TryParse(dateStart, out start) && DateTime.TryParse(dateEnd, out end))
            {
                var de = new CalendarDiaryEvent()
                {
                    Id = eventId,
                    DateStart = start,
                    DateEnd = end,
                };

                return Logic.Logic.Instance.CalendarDiaryEvent.UpdateDateStart(de).Success;
            }

            return false;
        }
    }
}