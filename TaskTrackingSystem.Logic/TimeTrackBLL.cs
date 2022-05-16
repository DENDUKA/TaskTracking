using System;
using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class TimeTrackBLL
    {
        #region singleton
        public static TimeTrackBLL Instance { get; private set; }

        private readonly ITimeTrackDAL timeTrackDAL;

        private TimeTrackBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    timeTrackDAL = new TimeTrackDAL();
                    break;
                case "EFDB":
                    timeTrackDAL = new EFDAL.TimeTrackDAL();
                    break;
            }
        }

        static TimeTrackBLL()
        {
            Instance = new TimeTrackBLL();
        }
        #endregion

        public List<TimeTrack> GetAll()
        {
            return timeTrackDAL.GetAll();
        }

        public bool Delete(int id)
        {
            return timeTrackDAL.Delete(id);
        }

        public int Create(TimeTrack timeTrack)
        {
            return timeTrackDAL.Create(timeTrack);
        }

        public bool Update(TimeTrack timeTrack)
        {
            return timeTrackDAL.Update(timeTrack);
        }

        public List<TimeTrack> GetAllForAccount(string login)
        {
            return timeTrackDAL.GetAllForAccount(login);
        }
    }
}