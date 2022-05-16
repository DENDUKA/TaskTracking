using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Project")]
    public class Project
    {
        public Project(int id, string name, Account account, ProjectType projectType, Status status, string contractStatus,
            DateTime dateStart, DateTime dateEnd, DateTime dateEndFact, string contractNumber, decimal cost, decimal paid, int companyId, bool premiumPaid, int storyPoints = 0)
        {
            this.Id = id;
            this.Name = name;
            this.Account = account;
            this.ProjectType = projectType;
            this.Status = status;
            this.ContractStatus = contractStatus;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.DateEndFact = dateEndFact;
            this.ContractNumber = contractNumber;
            this.Cost = cost;
            this.CompanyId = companyId;
            this.StoryPoints = storyPoints;
            this.Paid = paid;
            this.PremiumPaid = premiumPaid;
        }

        private Project()
        { }

        public static List<string> ContractStatusList = new List<string>()
        {
            "Нет", "Электронный отправлен", "Электронный получен", "Оригинал отправлен", "Оригинал получен"
        };

        [NotMapped]
        public static string DefaultContractStatus => ContractStatusList[0];

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Responsible { get; set; }

        [StringLength(100)]
        public string ContractStatus { get; set; }

        [ForeignKey("Responsible")]
        public Account Account { get; set; }

        [Column("ProjectType")]
        public int ProjectTypeId { get; set; }

        [ForeignKey("ProjectTypeId")]
        public virtual ProjectType ProjectType { get; set; }

        [Column("Status")]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateEnd { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateEndFact { get; set; }

        [StringLength(100)]
        public string ContractNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        [Column(TypeName = "money")]
        public decimal Paid { get; set; }

        [Required]
        public int CompanyId { get; set; }
        
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public bool PremiumPaid { get; set; }

        [NotMapped]
        public int StoryPoints { get; set; }

        public virtual ICollection<CalendarPlanItem> CalendarPlan { get; set; }

        public virtual ICollection<ProjectAccess> ProjectAccess { get; set; }

        public virtual ICollection<ProjectPathToFile> ProjectPathToFile { get; set; }

        public virtual ICollection<Settings> Settings { get; set; }

        public virtual ICollection<Task> Task { get; set; }
    }
}