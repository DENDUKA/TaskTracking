using System;
using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class CalendarBLL
    {

        #region singleton
        public static CalendarBLL Instance { get; private set; }

        private readonly ICalendayDiaryEventDAL calendarDAL;

        private CalendarBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    calendarDAL = new CalendarDAL();
                    break;
                case "Local":
                    calendarDAL = new CalendarLocalDAL();
                    break;
                case "EFDB":
                    calendarDAL = new EFDAL.CalendarDAL();
                    break;
            }
        }

        static CalendarBLL()
        {
            Instance = new CalendarBLL();
        }
        #endregion

        public CalendarDiaryEvent Get(int id)
        {
            return calendarDAL.Get(id);
        }
        public List<CalendarDiaryEvent> GetInRange(DateTime fromDate, DateTime toDate)
        {
            return calendarDAL.GetInRange(fromDate, toDate);
        }
        public OperationResult Delete(int id)
        {
            return calendarDAL.Delete(id);
        }
        public int Create(CalendarDiaryEvent diaryEvent)
        {
            return calendarDAL.Create(diaryEvent);
        }

        public OperationResult UpdateDateStart(CalendarDiaryEvent de)
        {
            return calendarDAL.UpdateDates(de);
        }
    }
}
