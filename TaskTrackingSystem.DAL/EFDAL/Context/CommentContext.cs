using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
	public class CommentContext : DbContext
	{
		public CommentContext() : base("AccountConnect")
		{ }

		public DbSet<Comment> Comments { get; set; }
	}
}