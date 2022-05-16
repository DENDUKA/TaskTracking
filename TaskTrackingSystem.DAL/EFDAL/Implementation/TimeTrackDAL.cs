using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class TimeTrackDAL : ITimeTrackDAL
    {
        public List<TimeTrack> GetAll()
        {
            using (var db = new TimeTrackContext())
            {
                return db.TimeTracks
                    .Include(x => x.Account)
                    .ToList();
            }
        }

        public List<TimeTrack> GetAllForAccount(string login)
        {
            using (var db = new TimeTrackContext())
            {
                return db.TimeTracks
                    .Where(x => x.Login == login)
                    .Include(x => x.Account)
                    .ToList();
            }
        }

        public bool Delete(int id)
        {
            using (var db = new TimeTrackContext())
            {
                var res = db.TimeTracks
                     .Where(x => x.Id == id)
                     .Delete();

                return res > 0;
            }
        }

        public int Create(TimeTrack timeTrack)
        {
            using (var db = new TimeTrackContext())
            {
                timeTrack.Account = null;

                if (string.IsNullOrEmpty(timeTrack.Description))
                {
                    timeTrack.Description = "---";
                }

                db.TimeTracks.Add(timeTrack);


                db.SaveChanges();
                return timeTrack.Id;
            }
        }

        public bool Update(TimeTrack timeTrack)
        {
            using (var db = new TimeTrackContext())
            {
                db.TimeTracks.AddOrUpdate(timeTrack);
                db.SaveChanges();
            }

            return true;
        }
    }
}