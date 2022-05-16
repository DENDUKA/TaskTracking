using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class CalendarPlanContext : DbContext
	{
		public CalendarPlanContext() : base("AccountConnect")
		{ }

		public DbSet<CalendarPlanItem> CalendarPlans { get; set; }
	}
}