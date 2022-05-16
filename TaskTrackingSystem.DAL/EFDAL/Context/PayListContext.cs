using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
    public class PayListContext : DbContext
    {
        public PayListContext() : base("AccountConnect")
        { }

        public DbSet<PayList> PayLists { get; set; }
    }
}
