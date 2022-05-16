using System.Linq;
using System.Web;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Access
{
    public class TaskAccess
    {
        private readonly Task task;
        private readonly Project project;
        private readonly string currentRole;
        private string Login => HttpContext.Current.User.Identity.Name;

        public TaskAccess(Task task, Project project, string currentRole)
        {
            this.task = task;
            this.project = project;
            this.currentRole = currentRole;

            var taskIsNew = task is null;//Задача создается


            if (taskIsNew || this.task.Status.Id != Shared.Entities.Status.DeletedId)
            {
                if (!taskIsNew)
                {
                    AccessToEdit();
                    AccessToDelete();
                    AccessToStatus();
                }
                AccessToStoryPointsEdit();
                AccessToStoryPointsView();
            }
            else
            {
                Edit = Status = false;
                AccessToDelete();
            }
            if (!taskIsNew)
            {
                AccessToRestore();
                AccessToChangeHistory();
            }
        }


        public bool StoryPointsEdit { get; private set; }
        public bool StoryPointsView { get; private set; }
        public bool Edit { get; private set; }
        public bool Delete { get; private set; }
        public bool Status { get; private set; }
        public bool Restore { get; private set; }
        public bool ChangeHistory { get; private set; }

        private void AccessToChangeHistory()
        {
            ChangeHistory = Role.Admin.Equals(currentRole);
        }

        private void AccessToStoryPointsEdit()
        {
            StoryPointsEdit = Role.AdmiBuhList.Contains(currentRole) ||
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole);
        }

        private void AccessToStoryPointsView()
        {
            StoryPointsView = Role.AdmiBuhList.Contains(currentRole)
                || Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole);
        }

        private void AccessToRestore()
        {
            if (task.Status.Id != Shared.Entities.Status.DeletedId)
            {
                Restore = false; //Если задача не удалена, то восстанавливать её не можем
                return;
            }

            Restore = Role.Admin.Equals(currentRole); //Восстанавливать может только админ
        }

        private void AccessToStatus()
        {
            Status =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                task.Account.Login.Equals(Login) || //Ответственный за задачу
                project.Account.Login.Equals(Login) || //Ответственный за проект
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole); //Ответственный за Категорию проектов
        }

        private void AccessToDelete()
        {
            Delete =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                //task.Account.Login.Equals(Login) || //Назначенный на задачу
                project.Account.Login.Equals(Login) || //Ответственный за проект
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole); //Ответственный за Категорию проектов
        }

        private void AccessToEdit()
        {
            Edit =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                task.Account.Login.Equals(Login) || //Ответственный за задачу
                project.Account.Login.Equals(Login) || //Ответственный за проект
                Role.ProjectTypeLeads[project.ProjectType.Id].Equals(currentRole); //Ответственный за Категорию проектов
        }
    }
}
