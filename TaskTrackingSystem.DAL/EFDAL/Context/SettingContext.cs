using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class SettingContext : DbContext
	{
		public SettingContext() : base("AccountConnect")
		{ }

		public DbSet<Settings> Settings { get; set; }
	}
}