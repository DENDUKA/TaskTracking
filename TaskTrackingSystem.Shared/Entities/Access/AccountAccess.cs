using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TaskTrackingSystem.Shared.Entities.Access
{
    [Table("AccountAccess")]
    public class AccountAccess
    {
        private AccountAccess()
        { }

        public AccountAccess(string accountLogin, List<ProjectTypeAccess> projectTypeAccesses = null, List<ProjectAccess> projectAccesses = null)
        {
            this.Login = accountLogin;
            this.ProjectTypeAccesses = projectTypeAccesses;
            this.ProjectAccesses = projectAccesses;
        }

        [Key]
        [Column(Order = 0)]
        [JsonIgnore]
        [StringLength(100)]
        public string Login { get; set; }

        [ForeignKey("Login")]
        public virtual Account Account { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "text")]
        public string AccessString { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<ProjectTypeAccess> ProjectTypeAccesses { get; set; }
        [NotMapped]
        public List<ProjectAccess> ProjectAccesses { get; set; }

        [NotMapped]
        public bool AdminPage { get; set; }
        [NotMapped]
        public bool UserListPage { get; set; }
        [NotMapped]
        public bool CalendarEdit { get; set; }
        [NotMapped]
        public bool CompanysPage { get; set; }
        [NotMapped]
        public bool AccountCreate { get; set; }
        [NotMapped]
        public bool AccountPasswordField { get; set; }
        [NotMapped]
        public bool AccountAccessPage { get; set; }
        [NotMapped]
        public bool CompanyEmployeeEditDelete { get; set; }
        [NotMapped]
        public bool CompanyEditDelete { get; set; }
        [NotMapped]
        public bool ProjectChangeHistory { get; set; }
    }
}