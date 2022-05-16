using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class CalendarDAL : ICalendayDiaryEventDAL
    {
        public int Create(CalendarDiaryEvent diaryEvent)
        {
            using (var db = new CalendarDiaryEventContext())
            {
                db.CalendarDEs
                    .Add(diaryEvent);
                db.SaveChanges();
            }

            return diaryEvent.Id;
        }

        public OperationResult Delete(int id)
        {
            using (var db = new CalendarDiaryEventContext())
            {
                var res = db.CalendarDEs
                     .Where(x => x.Id == id)
                     .Delete();

                return new OperationResult(res == 1);
            }
        }

        public CalendarDiaryEvent Get(int id)
        {
            using (var db = new CalendarDiaryEventContext())
            {
                return db.CalendarDEs
                     .Where(x => x.Id == id)
                     .FirstOrDefault();
            }
        }

        public List<CalendarDiaryEvent> GetInRange(DateTime fromDate, DateTime toDate)
        {
            using (var db = new CalendarDiaryEventContext())
            {
                return db.CalendarDEs
                     .Where(x => fromDate <= x.DateEnd && toDate >= x.DateStart)
                     .ToList();
            }
        }

        public OperationResult UpdateDates(CalendarDiaryEvent diaryEvent)
        {
            using (var db = new CalendarDiaryEventContext())
            {
                var dEvent = db.CalendarDEs.Where(x => x.Id == diaryEvent.Id).FirstOrDefault();
                var res = 0;
                if (dEvent != null)
                {
                    dEvent.DateEnd = diaryEvent.DateEnd;
                    dEvent.DateStart = diaryEvent.DateStart;

                    res = db.SaveChanges();
                }

                return new OperationResult(res == 1);
            }
        }
    }
}
