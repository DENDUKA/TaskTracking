using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL.PCOnline;
using TaskTrackingSystem.DAL.DAL.PCOnline;
using TaskTrackingSystem.Shared.Entities.PCOnline;

namespace TaskTrackingSystem.Logic.PCOnline
{
    public class PCOnlineBLL
    {
        #region singleton
        public static PCOnlineBLL Instance { get; private set; }

        private readonly IPCOnlineDAL pconlineDAL;

        private PCOnlineBLL()
        {
            pconlineDAL = new PCOnlineDAL();
        }

        static PCOnlineBLL()
        {
            Instance = new PCOnlineBLL();
        }
        #endregion

        public List<IpInfo> GetLocation()
        {
            return pconlineDAL.GetCurrentStatus();
        }
    }
}
