using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class ProjectTypeContext : DbContext
	{
		public ProjectTypeContext() : base("AccountConnect")
		{ }

		public DbSet<ProjectType> ProjectTypes { get; set; }
	}
}