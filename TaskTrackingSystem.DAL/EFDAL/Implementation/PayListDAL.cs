using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Implementation
{
    public class PayListDAL
    {
        public int Create(PayList payList)
        {
            using (var db = new PayListContext())
            {
                db.PayLists.Add(payList);
                db.SaveChanges();
                return payList.Id;
            }
        }

        public PayList Get(int id)
        {
            using (var db = new PayListContext())
            {
                return db.PayLists
                     .Where(x => x.Id == id)
                     .FirstOrDefault();
            }
        }

        public bool Update(PayList payList)
        {
            using (var db = new PayListContext())
            {

                db.PayLists.AddOrUpdate(payList);
                db.SaveChanges();
                return true;
            }
        }

        public List<PayList> GetForAccount(string login)
        {
            using (var db = new PayListContext())
            {
                var res = db.PayLists
                     .Where(x => x.Login == login)
                     .ToList();
                return res;
            }
        }

        public PayList GetForAccount(string login, DateTime date)
        {
            using (var db = new PayListContext())
            {
                return db.PayLists
                     .Where(x => x.Login == login && x.Date == date)
                     .FirstOrDefault();
                     
            }
        }

        public List<PayList> GetForPeriod(DateTime start, DateTime end)
        {
            using (var db = new PayListContext())
            {
                return db.PayLists
                     .Where(x => x.Date >= start && x.Date <= end)
                     .ToList();
            }
        }
    }
}