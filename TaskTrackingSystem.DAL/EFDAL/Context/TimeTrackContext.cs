using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class TimeTrackContext : DbContext
	{
		public TimeTrackContext() : base("AccountConnect")
		{ }

		public DbSet<TimeTrack> TimeTracks { get; set; }
	}
}