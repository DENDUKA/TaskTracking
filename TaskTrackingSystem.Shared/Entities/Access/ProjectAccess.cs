using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using TaskTrackingSystem.Shared.Attributes;

namespace TaskTrackingSystem.Shared.Entities.Access
{
    [Table("ProjectAccess")]
    public class ProjectAccess
    {
        public ProjectAccess(int id, string accountLogin, int projectId)
        {
            this.Id = id;
            this.AccountLogin = accountLogin;
            this.ProjectId = projectId;
        }

        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [StringLength(100)]
        [Column("AccountLogin")]
        public string AccountLogin { get; set; }

        [ForeignKey("AccountLogin")]
        public virtual Account Account { get; set; }

        [JsonIgnore]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string AccessString
        {
            get
            {
                return _accessString;
            }
            set
            {
                _accessString = value;

                ProjectAccess fromJson = null;

                if (_accessString != null)
                {
                    fromJson = JsonConvert.DeserializeObject<ProjectAccess>(_accessString);
                }

                if (fromJson != null)
                {
                    FillFields(fromJson);
                }
            }
        }

        [DisplayName("Создать новый")]
        [AccessFieldMetaData("pacreatenew", 1)]
        [NotMapped]
        public bool CreateNew { get; set; }

        [DisplayName("Редактировать")]
        [AccessFieldMetaData("paedit", 2)]
        [NotMapped]
        public bool Edit { get; set; }

        [DisplayName("Удалить")]
        [AccessFieldMetaData("padelete", 3)]
        [NotMapped]
        public bool Delete { get; set; }

        [DisplayName("Календарный план (Edit)")]
        [AccessFieldMetaData("pacalendarplanedit", 4)]
        [NotMapped]
        public bool CalendarPlanEdit { get; set; }

        [DisplayName("Оценка (View)")]
        [AccessFieldMetaData("pastorypointsfield", 5)]
        [NotMapped]
        public bool StoryPointsFieldTask { get; set; }

        [DisplayName("Ответственный")]
        [AccessFieldMetaData("paresponsible", 5)]
        [NotMapped]
        public bool Responsible { get; set; }

        [NotMapped]
        private string _accessString;

        private void FillFields(ProjectAccess pt)
        {
            //this.Id = pt.Id;
            //this.AccountLogin = pt.AccountLogin;
            //this.ProjectId = pt.ProjectId;
            this.CreateNew = pt.CreateNew;
            this.Edit = pt.Edit;
            this.Delete = pt.Delete;
            this.CalendarPlanEdit = pt.CalendarPlanEdit;
            this.StoryPointsFieldTask = pt.StoryPointsFieldTask;
            this.Responsible = pt.Responsible;
        }

        private ProjectAccess()
        {
        }
    }
}