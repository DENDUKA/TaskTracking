using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Access
{
    public class AccessBLL
    {
        #region singleton
        public static AccessBLL Instance { get; private set; }

       

        private AccessBLL()
        {

        }

        static AccessBLL()
        {
            Instance = new AccessBLL();
        }
        #endregion

        public CalendarAccess CalendarAccess()
        {
            return new CalendarAccess();
        }

        public ProjectTypeAccess ProjectTypeAccess(int projectTypeId, List<Project> projectList)
        {
            return new ProjectTypeAccess(projectTypeId, projectList);
        }

        public ProjectAccess ProjectAccess(Project project, List<Task> tasks, string role)
        {
            if (tasks is null)
                return new ProjectAccess(project, role);
            else
                return new ProjectAccess(project, tasks, role);
        }

        public TaskAccess TaskAccess(Task task, Project project, string role)
        {
            return new TaskAccess(task, project, role);
        }
    }
}