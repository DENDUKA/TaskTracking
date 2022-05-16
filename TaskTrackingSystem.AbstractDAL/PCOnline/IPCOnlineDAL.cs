using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities.PCOnline;

namespace TaskTrackingSystem.AbstractDAL.PCOnline
{
    public interface IPCOnlineDAL
    {
        List<IpInfo> GetCurrentStatus();
    }
}
