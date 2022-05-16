using System;
using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.DAL;
using TaskTrackingSystem.Logic.Access;
using TaskTrackingSystem.Shared.Entities;
using Task = TaskTrackingSystem.Shared.Entities.Task;

namespace TaskTrackingSystem.Logic.Email
{
    public static class EmailBLL
    {
        internal static void TaskCreated(Task task)
        {
            var account = AccountBLL.Instance.Get(task.AccountLogin);
            SendEmail.TaskCreation(task, account.Email);
        }

        internal static void ProjectCreated(Project project)
        {
            var account = Logic.Instance.Account.Get(project.Responsible);
            SendEmail.ProjectCreation(project, account.Email);
        }

        internal static void TaskChanged(Task task, List<string> changes)
        {
            task = Logic.Instance.Task.Get(task.Id);
            SendEmail.TaskChanged(task, task.Account.Email , changes);
        }

        internal static void ProjectChanged(Project project, List<string> changes)
        {
            SendEmail.ProjectChanged(project, project.Account.Email, changes);
        }

        internal static void TaskEndTime(Task task)
        {
            var emailList = new List<string>() { task.Account.Email };
            task.Project.ProjectAccess = AccountAccessBLL.Instance.GetAccessForProject(task.ProjectId);
            emailList.AddRange(task.Project.ProjectAccess.Where(x => x.Responsible).Select(x => x.Account.Email));

            SendEmail.TaskEndTime(task, emailList.Distinct().ToList());
        }

        internal static void ProjectEndTime(Project project)
        {
            var emailList = new List<string>() { AccountBLL.Instance.Get("G1Uker").Email, };

            SendEmail.ProjectEndTime(project, emailList.Distinct().ToList());
        }

        internal static void OverdueTaskTime(Task t)
        {
            var resps = ProjectBLL.Instance.GetResponsibles(t.ProjectId);

            SendEmail.OverdueTaskTimeToResp(t, resps.Select(x => x.Email).ToList());
        }

        internal static void OverdueProjectTime(Project project)
        {
            var emailList = new List<string>() { AccountBLL.Instance.Get("G1Uker").Email, };

            SendEmail.OverdueProjectTime(project, emailList);
        }

        internal static void SendTaskClosed(Task task, Task oldTask)
        {
            if (task.Status.Id == Status.DoneId && oldTask.Status.Id != Status.DoneId)
            {
                if (task.Reporter == null)
                {
                    task = TaskBLL.Instance.Get(task.Id);
                }

                SendEmail.ToAuthorTaskClosed(task, task.Reporter.Email);

                var responobles = ProjectBLL.Instance.GetResponsibles(task.ProjectId);

                foreach (var acc in responobles)
                {
                    SendEmail.ToResponsibleTaskClosed(task, acc.Email);
                }
            }
        }
    }
}