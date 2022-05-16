using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Task")]
    public class Task
    {
        public Task(int id, string name, DateTime dateStart, DateTime dateEnd, Status status, int projectId, string projectName, Account account, string description, Account reporter, int storyPoints = 0)
        {
            this.Id = id;
            this.Name = name;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.Status = status;
            this.ProjectId = projectId;
            this.Account = account;
            this.Description = description;
            this.StoryPoints = storyPoints;
            this.Reporter = reporter;
        }

        private Task()
        { }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateEnd { get; set; }

        [Column("Status")]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [Column("Project")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [StringLength(100)]
        [Column("Account")]
        public string AccountLogin { get; set; }

        [ForeignKey("AccountLogin")]
        public virtual Account Account { get; set; }

        [StringLength(100)]
        [Column("Reporter")]
        public string ReporterLogin { get; set; }

        [ForeignKey("ReporterLogin")]
        public virtual Account Reporter { get; set; }

        public string Description { get; set; }

        public int StoryPoints { get; set; }

        public virtual List<Comment> Comment { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsApproved { get; set; }

    }
}
