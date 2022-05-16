using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
    public class TaskContext : DbContext
    {
        public TaskContext() : base("AccountConnect")
        { }

        public DbSet<Task> Tasks { get; set; }
    }
}