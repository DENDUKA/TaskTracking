using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class StatusBLL
    {
        #region singleton
        public static StatusBLL Instance { get; private set; }

        private readonly IStatusDAL statusDAL;

        private StatusBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    statusDAL = new StatusDAL();
                    break;
                case "Local":
                    statusDAL = new StatusLocalDAL();
                    break;
                case "EFDB":
                    statusDAL = new EFDAL.StatusDAL();
                    break;
            }
        }

        static StatusBLL()
        {
            Instance = new StatusBLL();
        }
        #endregion

        public List<Status> GetAll()
        {
            return statusDAL.GetAll();
        }

        public Status Get(int id)
        {
            return statusDAL.Get(id);
        }

        public void ClearCache()
        {
            statusDAL.ClearCache();
        }

        public string GetColor(string statusName)
        {
            switch (statusName)
            {
                case Status.Postponded:
                    return "text-warning";
                case Status.InWork:
                    return "text-success";
                case Status.Done:
                    return "text-danger";
                default:
                    return "text-dark";
            }
        }
    }
}