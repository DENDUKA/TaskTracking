using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public 	class StatusContext : DbContext
	{
		public StatusContext() : base("AccountConnect")
		{ }

		public DbSet<Status> Statuses { get; set; }
	}
}