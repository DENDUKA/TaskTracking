using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
    public class ActionLogsContext : DbContext
	{
		public ActionLogsContext() : base("AccountConnect")
		{ }

		public DbSet<ActionLog> ActionLogs { get; set; }
	}
}
