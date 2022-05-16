using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [NotMapped]
    public class CalendarPlan
    {
        public CalendarPlan(int projectId, List<CalendarPlanItem> calendarPlanItems)
        {
            this.CalendarPlanItems = calendarPlanItems;
            this.ProjectId = projectId;
        }

        public int ProjectId { get; set; }

        public List<CalendarPlanItem> CalendarPlanItems { get; set; }
    }
}