using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Access
{
    public class ProjectAccess
    {
        private readonly Project project;
        private readonly string currentRole;
        private string Login => HttpContext.Current.User.Identity.Name;

        public Dictionary<int, TaskAccess> AccessToTasks = new Dictionary<int, TaskAccess>();

        public ProjectAccess(Project project, string currentRole)
        {
            this.project = project;

            this.currentRole = currentRole;

            Init();
        }

        public ProjectAccess(Project project, List<Task> tasks, string currentRole)
        {
            this.project = project;

            this.currentRole = currentRole;

            Init();
            GetAccessForTasks(tasks);
        }

        private void GetAccessForTasks(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                AccessToTasks.Add(task.Id, new TaskAccess(task, project, currentRole));
            }
        }

        private void Init()
        {
            AccessToEdit();
            AccessToDelete();
            AccessToCostField();
            AccessToCreateNewTask();
        }

        public bool Edit { get; private set; }
        public bool Delete { get; private set; }
        public bool CostField { get; private set; }
        public bool CreateNewTask { get; private set; }

        private void AccessToCreateNewTask()
        {
            if (project is null ? false : project.Status.Id == Status.DeletedId)
            {
                CreateNewTask = false; //Если статус удален, задачу для этого проекта создать никто не может
                return;
            }


            CreateNewTask =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                (project is null ? false : (project.Account.Login.Equals(Login) || //Ответственный за проект
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole))); //Если ответственный за Категорию проектов
        }

        private void AccessToEdit()
        {
            Edit =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                (project is null ? false : (project.Account.Login.Equals(Login) || //Если ответственный за проект
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole))); //Если ответственный за Категорию проектов
        }
        private void AccessToDelete()
        {
            Delete =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                (project is null ? false : (project.Account.Login.Equals(Login) || //Если ответственный за проект
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole))); //Если ответственный за Категорию проектов
        }

        private void AccessToCostField()
        {
            CostField = Role.AdmiBuhList.Contains(currentRole);
        }
    }
}
