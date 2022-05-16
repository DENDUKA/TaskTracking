using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class CalendarDiaryEventContext : DbContext
	{
		public CalendarDiaryEventContext() : base("AccountConnect")
		{ }

		public DbSet<CalendarDiaryEvent> CalendarDEs { get; set; }
	}
}
