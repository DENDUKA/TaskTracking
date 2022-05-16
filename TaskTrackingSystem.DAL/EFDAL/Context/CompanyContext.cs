using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class CompanyContext : DbContext
	{
		public CompanyContext() : base("AccountConnect")
		{ }

		public DbSet<Company> Companys { get; set; }
	}
}