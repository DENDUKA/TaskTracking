using Microsoft.AspNet.SignalR;
using TaskTrackingSystem.Logic;

namespace TaskTrackingSystem.SignalR.Hubs
{
    public class GeneralHub : Hub
    {
        public void PremiumPaid(int projectId, bool isPaid)
        {
            var project = ProjectBLL.Instance.Get(projectId);
            if (project != null)
            {
                project.PremiumPaid = isPaid;
                ProjectBLL.Instance.Update(project);
            }
        }
    }
}