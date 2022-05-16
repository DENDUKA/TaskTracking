using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public 	class ProjectTypeAccessContext : DbContext
	{
		public ProjectTypeAccessContext() : base("AccountConnect")
		{ }

		public DbSet<ProjectTypeAccess> ProjectTypeAccesses { get; set; }
	}
}
