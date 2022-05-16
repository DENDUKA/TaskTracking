using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackingSystem.DAL.EFDAL.Implementation;
using TaskTrackingSystem.Shared.Entities.Network;

namespace TaskTrackingSystem.Logic.Network
{
    public class PCNetworkInfoBLL
    {
        #region singleton
        public static PCNetworkInfoBLL Instance { get; private set; }

        private PCNetworkInfoBLL()
        {
        }

        static PCNetworkInfoBLL()
        {
            Instance = new PCNetworkInfoBLL();
        }
        #endregion

        private PCNetworkInfoDAL pcNetworkInfoDAL = new PCNetworkInfoDAL();

        public int Save(PCNetworkInfo pcni)
        {
            var acc = AccountBLL.Instance.Get(pcni.AccountLogin);

            if (acc is null)
            {
                pcni.AccountLogin = null;
            }

            if (pcni.Id == 0)
            {
                var sameIp = pcNetworkInfoDAL.GetByIp(pcni.Ip);
                if (sameIp == null)
                {
                    return pcNetworkInfoDAL.Create(pcni);
                }
                else
                {
                    pcni.Id = sameIp.Id;                   
                }
            }

            pcNetworkInfoDAL.Update(pcni);
            return pcni.Id;
        }

        public List<PCNetworkInfo> GetAll()
        {
            return pcNetworkInfoDAL.GetAll();
        }



        public bool Delete(int id)
        {

            return pcNetworkInfoDAL.Delete(id);
        }

        public List<PCNetworkInfo> UpdateStatus(string[] ips)
        {
            return null;
        }

        public List<PCNetworkInfo> UpdateStatus(string ip)
        {
            return null;
        }
    }
}
