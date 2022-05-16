using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class StatusLocalDAL : IStatusDAL
    {
        public void ClearCache()
        {
            
        }

        public Status Get(int id)
        {
            return LocalStorage.Statuses[id];
        }

        public List<Status> GetAll()
        {
            return LocalStorage.Statuses.Values.ToList();
        }
    }
}
