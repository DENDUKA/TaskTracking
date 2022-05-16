using System;
using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Logic.Email;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class ProjectBLL
    {
        #region singleton
        public static ProjectBLL Instance { get; private set; }

        private readonly IProjectDAL projectDAL;
        private readonly ICalendarPlanDAL calendarPlanDAL;

        private ProjectBLL()
        {
            calendarPlanDAL = new CalendarPlanDAL();

            projectDAL = new EFDAL.ProjectDAL();

            switch (DAL.Shared.DALType)
            {
                case "DB":
                    calendarPlanDAL = new CalendarPlanDAL();
                    break;
                case "Local":
                    calendarPlanDAL = new CalendarPlanDAL();
                    break;
                case "EFDB":
                    calendarPlanDAL = new CalendarPlanDAL();
                    break;
            }

            switch (DAL.Shared.DALType)
            {
                case "DB":
                    projectDAL = new ProjectDAL();
                    break;
                case "Local":
                    projectDAL = new ProjectLocalDAL();
                    break;
                case "EFDB":
                    projectDAL = new EFDAL.ProjectDAL();
                    break;
            }

            //switch (DAL.Shared.DALType)
            //{
            //    case "DB":
            //        projectDAL = new ProjectDAL();
            //        break;
            //    case "Local":
            //        projectDAL = new ProjectLocalDAL();
            //        break;
            //}
        }

        static ProjectBLL()
        {
            Instance = new ProjectBLL();
        }
        #endregion

        //1/1/1753 12:00:00
        //private DateTime minSQLTime = new DateTime(1760,1,1);

        public bool MarkAsPaid(int id)
        {
            var project = Get(id);
            if (project != null)
            {
                project.PremiumPaid = true;
                Update(project);
                return true;
            }
            return false;
        }

        public List<Project> GetAllForProjectType(int projTypeId)
        {
            var projects = projectDAL.GetAllForProjectType(projTypeId);

            return projects;
        }
        public List<Project> GetAllForCompany(int companyId)
        {
            return projectDAL.GetAllForCompany(companyId).Where(x => x.Status.Name != Status.Deleted).ToList();
        }

        public List<Project> GetAllForProjectType(int[] projTypeIds)
        {
            var projects = new List<Project>();

            foreach (var ptId in projTypeIds)
            {
                projects.AddRange(GetAllForProjectType(ptId));
            }

            return projects;
        }

        public Project Get(int id)
        {
            var project = projectDAL.Get(id);            
            return project;
        }

        public int Create(Project project)
        {
            ProjectDone(project);            

            var pId = projectDAL.Create(project);

            project = projectDAL.Get(pId);

            var pAccess = new Shared.Entities.Access.ProjectAccess(0, project.Account.Login, pId)
            {
                Responsible = true
            };

            Logic.Instance.AccountAccess.ProjectAdd(pAccess);

            if (pId != 0)
            {                
                EmailBLL.ProjectCreated(project);
            }

            return pId;
        }

        public OperationResult Delete(int id)
        {
            var project = Get(id);

            if (project is null)
                return new OperationResult(false, "Проекта с таким ID не найдено");

            if (project.Status.Id.Equals(Status.DeletedId))
            {
                foreach (var x in Logic.Instance.Task.GetAllForProject(id, true))
                {
                    Logic.Instance.Task.FullDelete(x.Id);
                }

                return FullDelete(id);
            }
            else
            {
                foreach (var x in Logic.Instance.Task.GetAllForProject(id, false))
                {
                    Logic.Instance.Task.Delete(x.Id);
                }

                return projectDAL.UpdateStatus(id, Status.DeletedId);
            }
        }

        public OperationResult FullDelete(int id)
        {
            return projectDAL.Delete(id);
        }

        public OperationResult Restore(int id)
        {
            var result = projectDAL.UpdateStatus(id, Status.PostpondedId);

            if (result.Success)
            {
                foreach (var x in Logic.Instance.Task.GetAllForProject(id, true))
                {
                    Logic.Instance.Task.Restore(x.Id);
                }
            }
            else
            {
                return result;
            }

            return new OperationResult(true);
        }

        public void ChangeProjectType(int projectId, int projectTypeId)
        {
            if (projectId != 0 && projectTypeId != 0)
            {
                projectDAL.ChangeProjectType(projectId, projectTypeId);
            }
        }

        public List<Project> GetAllForAccount(string login, bool selectDeleted = false)
        {
            var projects = projectDAL.GetAllForAccount(login);

            if (!selectDeleted)
            {
                projects = projects.FindAll(x => x.Status.Id != Status.DeletedId);
            }

            return projects;
        }

        public void Update(Project project)
        {
            var oldProject = Logic.Instance.Project.Get(project.Id);

            if (project.Status.Id == Status.DoneId && oldProject.Status.Id != Status.DoneId)
            {
                project.DateEndFact = DateTime.Now;
            }

            var result = projectDAL.Update(project);
            project = Get(project.Id);

            if (result)
            {
                Logic.Instance.ActionLogs.LogProjectAction(project, oldProject);
            }
        }

        private void ProjectDone(Project project)
        {
            if (project.Status.Id == Status.DoneId)
            {
                project.DateEndFact = DateTime.Now;
            }
            else
            {
                project.DateEndFact = DALHelper.MinDateTimeSQL;
            }
        }

        public List<ProjectPathToFile> GetAllFilePaths(int id)
        {
            return projectDAL.GetFilePaths(id);
        }

        public int CreateFilePath(int projectId)
        {
            return projectDAL.CreateFilePath(projectId);
        }
        public OperationResult DeleteFilePath(int filePathId)
        {
            return projectDAL.DeleteFilePath(filePathId);
        }
        public OperationResult UpdateFilePath(ProjectPathToFile filePath)
        {
            if (!filePath.Path.Contains(Helper.FtpRoot))
            {
                filePath.Path = Helper.ChangeRoot(filePath.Path);
            }

            return projectDAL.UpdateFilePath(filePath);
        }

        public List<Account> GetResponsibles(int projectId)
        {
            var accountList = new List<Account>();

            var projectAccesses = Logic.Instance.AccountAccess.GetAccessForProject(projectId);

            foreach (var x in projectAccesses.Where(x => x.Responsible))
            {
                accountList.Add(AccountBLL.Instance.Get(x.AccountLogin));
            }

            return accountList;
        }

        public string GetResponsiblesString(Project project)
        {
            var result = "";

            if (project.ProjectAccess is null)
            {
                project.ProjectAccess = Logic.Instance.AccountAccess.GetAccessForProject(project.Id);
            }


            foreach (var x in project.ProjectAccess.Where(x => x.Responsible))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += ",   ";
                }

                var name = Logic.Instance.Account.Get(x.AccountLogin).Name;

                result += name;
            }

            return result;
        }

        #region CalendarPlan
        public CalendarPlan GetCalendarPlan(int projectId)
        {
            return calendarPlanDAL.Get(projectId);
        }

        public int CreateCalendarPlanItem(int projectId)
        {
            return calendarPlanDAL.CreateCalendarPlanItem(projectId);
        }
        public OperationResult UpdateCalendarPlanItem(CalendarPlanItem calendarPlanItem)
        {
            return calendarPlanDAL.UpdateCalendarPlanItem(calendarPlanItem);
        }
        public OperationResult DeleteCalendarPlanItem(int calendarPlanItemId)
        {
            return calendarPlanDAL.DeleteCalendarPlanItem(calendarPlanItemId);
        }

        #endregion
    }
}
