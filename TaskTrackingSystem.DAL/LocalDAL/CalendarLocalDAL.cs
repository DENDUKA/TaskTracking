using System;
using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class CalendarLocalDAL : ICalendayDiaryEventDAL
    {
        public int Create(CalendarDiaryEvent diaryEvent)
        {
            throw new NotImplementedException();
        }

        public OperationResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CalendarDiaryEvent Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<CalendarDiaryEvent> GetInRange(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public OperationResult UpdateDates(CalendarDiaryEvent diaryEvent)
        {
            throw new NotImplementedException();
        }
    }
}
