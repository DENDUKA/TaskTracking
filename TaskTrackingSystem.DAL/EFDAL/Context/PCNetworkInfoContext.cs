using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities.Network;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
    public class PCNetworkInfoContext : DbContext
    {
        public PCNetworkInfoContext() : base("AccountConnect")
        { }

        public DbSet<PCNetworkInfo> PCNetworkInfos { get; set; }
    }
}
