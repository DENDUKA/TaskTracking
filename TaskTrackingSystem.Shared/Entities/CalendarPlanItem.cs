using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("CalendarPlan")]
    public class CalendarPlanItem
    {
        public CalendarPlanItem(int id, int stageNum, string workType, string deadlines)
        {
            this.StageNum = stageNum;
            this.WorkType = workType;
            this.Deadlines = deadlines;
            this.Id = id;
        }

        private CalendarPlanItem()
        { }

        [Key]
        public int Id { get; set; }

        public int StageNum { get; set; }

        [Required]
        public string WorkType { get; set; }

        [Required]
        public string Deadlines { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }
}