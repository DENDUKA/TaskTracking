using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ICalendarPlanDAL
    {
        CalendarPlan Get(int projectId);
        int CreateCalendarPlanItem(int projectId);
        OperationResult UpdateCalendarPlanItem(CalendarPlanItem calendarPlanItem);
        OperationResult DeleteCalendarPlanItem(int calendarPlanItemId);
    }
}
