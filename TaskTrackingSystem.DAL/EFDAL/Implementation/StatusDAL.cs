using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.EFDAL
{
    public class StatusDAL : IStatusDAL
    {
        private Dictionary<int, Status> statusCache = new Dictionary<int, Status>();
        private bool getAll = false;


        public Status Get(int id)
        {
            if (statusCache.ContainsKey(id))
            {
                return (Status)statusCache[id].Clone();
            }

            var status = GetFromDB(id);

            if (status is null)
                return null;

            statusCache.Add(id, status);

            return (Status)status.Clone();
        }

        private Status GetFromDB(int id)
        {
            using (var db = new StatusContext())
            {
                return db.Statuses
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        public List<Status> GetAll()
        {
            if (!getAll)
            {
                statusCache.Clear();

                using (var db = new StatusContext())
                {
                    foreach (var s in db.Statuses)
                    {
                        statusCache.Add(s.Id, s);
                    }
                }

                getAll = true;
            }

            return statusCache.Select(status => (Status)status.Value.Clone()).ToList();
        }

        public void ClearCache()
        {
            statusCache.Clear();
            getAll = false;
        }
    }
}
