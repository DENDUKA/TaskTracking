using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public 	class ProjectPathToFileContext : DbContext
	{
		public ProjectPathToFileContext() : base("AccountConnect")
		{ }

		public DbSet<ProjectPathToFile> ProjectPathToFiles { get; set; }
	}
}
