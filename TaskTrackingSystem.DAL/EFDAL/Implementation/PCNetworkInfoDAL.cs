using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.Shared.Entities.Network;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.DAL.EFDAL.Implementation
{
    public class PCNetworkInfoDAL
    {
        public int Create(PCNetworkInfo pcni)
        {
            using (var db = new PCNetworkInfoContext())
            {
                db.PCNetworkInfos.Add(pcni);
                db.SaveChanges();
                return pcni.Id;
            }
        }

        public List<PCNetworkInfo> GetAll()
        {
            using (var db = new PCNetworkInfoContext())
            {
                return db.PCNetworkInfos.ToList();                
            }
        }
        public PCNetworkInfo Get(int id)
        {
            using (var db = new PCNetworkInfoContext())
            {
                return db.PCNetworkInfos.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public PCNetworkInfo GetByIp(string ip)
        {
            using (var db = new PCNetworkInfoContext())
            {
                return db.PCNetworkInfos.Where(x => x.Ip == ip).FirstOrDefault();
            }
        }

        public bool Update(PCNetworkInfo pcni)
        {
            using (var db = new PCNetworkInfoContext())
            {
                db.PCNetworkInfos.AddOrUpdate(pcni);
                return db.SaveChanges() > 0;
            }
        }
        public bool Delete(int id)
        {
            using (var db = new PCNetworkInfoContext())
            {
                var count = db.PCNetworkInfos
                     .Where(x => x.Id == id)
                     .Delete();

                return count > 0;
            }
        }
    }
}
