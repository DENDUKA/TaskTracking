using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
    public class AccountAccessContext : DbContext
    {
        public AccountAccessContext() : base("AccountConnect")
        { }

        public DbSet<AccountAccess> AccountAccesses { get; set; }
    }
}