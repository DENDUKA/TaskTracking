using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using TaskTrackingSystem.Shared.Attributes;

namespace TaskTrackingSystem.Shared.Entities.Access
{
    [Table("ProjectTypeAccess")]
    public class ProjectTypeAccess
    {
        public ProjectTypeAccess(int id, string accountLogin, int projectTypeId)
        {
            this.Id = id;
            this.AccountLogin = accountLogin;
            this.ProjectTypeId = projectTypeId;
        }

        [Key]
        [JsonIgnore]        
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [StringLength(100)]
        public string AccountLogin { get; set; }

        [ForeignKey("AccountLogin")]
        public virtual Account Account { get; set; }

        [JsonIgnore]
        public int ProjectTypeId { get; set; }

        [ForeignKey("ProjectTypeId")]
        public virtual ProjectType ProjectType { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string AccessString { get; set; }



        [DisplayName("Создать новый")]
        [AccessFieldMetaData("ptacreatenew", 1)]
        [NotMapped]
        public bool CreateNew { get; set; }
        [DisplayName("Редактировать")]
        [AccessFieldMetaData("ptaedit", 2)]
        [NotMapped]
        public bool Edit { get; set; }
        [DisplayName("Удалить")]
        [AccessFieldMetaData("ptadelete", 3)]
        [NotMapped]
        public bool Delete { get; set; }
        [DisplayName("ExelReport")]
        [AccessFieldMetaData("ptaexelreport", 4)]
        [NotMapped]
        public bool ExelReport { get; set; }
        [DisplayName("Редактирование доступа к проектам")]
        [AccessFieldMetaData("paprojectaccesspage", 6)]
        [NotMapped]
        public bool ProjectAccessPage { get; set; }
        [DisplayName("Проект -> Задача -> Оценка (View)")]
        [AccessFieldMetaData("paprojectstorypointfield", 7)]
        [NotMapped]
        public bool StoryPointsFieldTask { get; set; }
        [DisplayName("Проект -> Стоимость (View)")]
        [AccessFieldMetaData("pacostfieldproject", 8)]
        [NotMapped]
        public bool CostFieldProject { get; set; }
        [DisplayName("Категория проектов -> Удаленные проекты")]
        [AccessFieldMetaData("padeletedprojectaccess", 9)]
        [NotMapped]
        public bool DeletedProjectAccess { get; set; }
        [DisplayName("Задача -> Восстановить задачу")]
        [AccessFieldMetaData("parestoretaskproject", 9)]
        [NotMapped]
        public bool RestoreTask { get; set; }
        [DisplayName("Ответственный")]
        [AccessFieldMetaData("paresponsible", 10)]
        [NotMapped]
        public bool Responsible { get; set; }
        [DisplayName("Задача -> История изменений")]
        [AccessFieldMetaData("pachengehistorytask", 11)]
        [NotMapped]
        public bool ChengeHistoryTask { get; set; }

        private ProjectTypeAccess()
        { }
    }
}