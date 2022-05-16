using TaskTrackingSystem.Logic;

namespace TaskTrackingSystem.SignalR.Hubs
{
    public class TaskHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Approved(int id, bool isApprove)
        {
            var task = TaskBLL.Instance.Get(id);
            task.IsApproved = isApprove;
            TaskBLL.Instance.Update(task);
        }

        public void ChangeStatus(int id, int statusId)
        {
            var task = TaskBLL.Instance.Get(id);
            task.StatusId = statusId;
            TaskBLL.Instance.Update(task);
        }
    }
}