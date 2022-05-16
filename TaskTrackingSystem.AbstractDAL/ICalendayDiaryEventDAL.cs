using System;
using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ICalendayDiaryEventDAL
    {
        CalendarDiaryEvent Get(int id);
        List<CalendarDiaryEvent> GetInRange(DateTime fromDate, DateTime toDate);
        OperationResult Delete(int id);
        OperationResult UpdateDates(CalendarDiaryEvent diaryEvent);
        int Create(CalendarDiaryEvent diaryEvent);
    }
}