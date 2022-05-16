using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class EmployeeContext : DbContext
	{
		public EmployeeContext() : base("AccountConnect")
		{ }

		public DbSet<Employee> Employees { get; set; }
	}
}
