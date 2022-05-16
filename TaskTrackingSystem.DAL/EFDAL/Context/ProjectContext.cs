using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class ProjectContext : DbContext
	{
		public ProjectContext() : base("AccountConnect")
		{ }

		public DbSet<Project> Projects { get; set; }
	}
}