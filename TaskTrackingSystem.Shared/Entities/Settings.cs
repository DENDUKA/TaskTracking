using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Settings")]
    public class Settings
    {
        public Settings(string sysAdmin, int projectIdForSysAdmin)
        {
            this.SystemAdminLogin = sysAdmin;
            this.ProjectIdForSystemAdmin = projectIdForSysAdmin;
        }

        private Settings()
        { }

        [Key]
        [StringLength(100)]
        [Column("SystemAdmin")]
        public string SystemAdminLogin { get; set; }

        [ForeignKey("SystemAdminLogin")]
        public virtual Account Account { get; set; }
        
        public int ProjectIdForSystemAdmin { get; set; }

        [ForeignKey("ProjectIdForSystemAdmin")]
        public virtual Project Project { get; set; }
    }
}