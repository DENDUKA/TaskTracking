using System.Data.Entity;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.Shared.Entities.Access;
using TaskTrackingSystem.Shared.Entities.Network;

namespace TaskTrackingSystem.DAL.EFDAL.Context
{
    public class AccountContext : DbContext
	{
		public AccountContext() : base("name=AccountConnect")
		{ }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CalendarPlanItem> CalendarPlan { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectAccess> ProjectAccess { get; set; }
        public virtual DbSet<ProjectPathToFile> ProjectPathToFile { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<ProjectTypeAccess> ProjectTypeAccess { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TimeTrack> TimeTrack { get; set; }
        public virtual DbSet<AccountAccess> AccountAccess { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        public virtual DbSet<CalendarDiaryEvent> CalendarDiaryEvent { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<PayList> Paylists { get; set; }
        public virtual DbSet<PCNetworkInfo> PCNetworkInfo { get; set; }
    }
}

