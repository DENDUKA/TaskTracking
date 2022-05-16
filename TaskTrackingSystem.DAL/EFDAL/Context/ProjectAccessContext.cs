using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public 	class ProjectAccessContext : DbContext
	{
		public ProjectAccessContext() : base("AccountConnect")
		{ }

		public DbSet<ProjectAccess> ProjectAccesses { get; set; }
	}
}
