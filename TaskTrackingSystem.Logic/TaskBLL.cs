using System;
using System.Collections.Generic;
using System.Web;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL;
using TaskTrackingSystem.Logic.Email;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class TaskBLL
    {
        #region singleton
        public static TaskBLL Instance { get; private set; }

        private readonly ITaskDAL taskDAL;

        private TaskBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    taskDAL = new TaskDAL();
                    break;
                case "Local":
                    taskDAL = new TaskLocalDAL();
                    break;
                case "EFDB":
                    taskDAL = new EFDAL.TaskDAL();
                    break;
            }
        }

        static TaskBLL()
        {
            Instance = new TaskBLL();
        }
        #endregion

        public Task Get(int id)
        {
            return taskDAL.Get(id);
        }

        public List<Task> GetAll(DateTime start, DateTime end, int[] statusIds)
        {
            return taskDAL.GetAll(start, end, statusIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="selectDeleted">Возвращать удаленные задачи? (только для админа)</param>
        /// <returns></returns>
        public List<Task> GetAllForProject(int projectId, bool selectDeleted = false)
        {
            var tasks = taskDAL.GetAllForProject(projectId);

            if (selectDeleted)
            {
                return tasks;
            }
            else
            {
                return tasks.FindAll(x => x.Status.Name != Status.Deleted);
            }
        }

        public int Add(Task task)
        {
            if (string.IsNullOrEmpty(task.Name))
                return 0;
            if (string.IsNullOrEmpty(task.Description))
            {
                task.Description = "";
            }

            task.Reporter = new Account("", HttpContext.Current.User.Identity.Name);
            task.ReporterLogin = task.Reporter?.Login;

            var id = taskDAL.Add(task);
            task = taskDAL.Get(id);

            if (id != 0)
            {
                EmailBLL.TaskCreated(task);
            }

            return id;
        }

        public List<Task> GetAllForAccount(string login, bool selectDeleted = false)
        {
            var tasks = taskDAL.GetAllForAccount(login);

            if (!selectDeleted)
            {
                tasks = tasks.FindAll(x => x.Status.Name != Status.Deleted);
            }

            return tasks;
        }

        public bool UpdateStatus(int id, int statusId)
        {
            var oldTask = Get(id);

            oldTask.StatusId = statusId;
            oldTask.Status.Id = statusId;
            oldTask.Status.Name = Status.All[statusId].Name;
            
            oldTask.Project.Status = null;
            oldTask.Project = null;

            oldTask.Status.Project = null;
            oldTask.Status.Task = null;

            return Update(oldTask);
        }

        public bool Update(Task task)
        {
            if (string.IsNullOrEmpty(task.Description))
            {
                task.Description = "";
            }

            var oldTask = Logic.Instance.Task.Get(task.Id);

            if (oldTask.StoryPoints != 0 && task.StoryPoints == 0)
            {
                task.StoryPoints = oldTask.StoryPoints;
            }

            TaskChangeStatus(task, oldTask);

            var result = taskDAL.UpdateMainFiels(task);
            task = taskDAL.Get(task.Id);

            if (result)
            {
                Logic.Instance.ActionLogs.LogTaskAction(task, oldTask);
            }

            return result;
        }

        private void TaskChangeStatus(Task newTask, Task oldTask)
        {
            if (newTask.StatusId == Status.DoneId && oldTask.StatusId != Status.DoneId)
            {
                newTask.DateEnd = DateTime.Now.Date;
            }
        }

        public void StatusChanged(Task task)
        {

        }

        public OperationResult FullDelete(int id)
        {
            return taskDAL.Delete(id);
        }
        
        public OperationResult Delete(int id)
        {
            var task = Get(id);

            if (task is null)
            {
                return new OperationResult(true, "Задача с таким ID не найдена");
            }

            if (task.Status.Id.Equals(Status.DeletedId))
            {
                return FullDelete(id);
            }
            else
            {
                return taskDAL.UpdateStatus(id, Status.DeletedId);
            }
        }

        public OperationResult Restore(int id)
        {
            var task = Get(id);

            if (task == null)
            {
                return new OperationResult(false, "Задача с таким не найдена");
            }

            if (!task.Status.Name.Equals(Status.Deleted))
            {
                return new OperationResult(true);
            }

            var project = Logic.Instance.Project.Get(task.ProjectId);

            if (project == null)
            {
                return new OperationResult(false, "Проект для этой задачи не найден");
            }

            if (project.Status.Name.Equals(Status.Deleted))
            {
                return new OperationResult(false, "Проект для этой задачи удален");
            }

            return taskDAL.UpdateStatus(id, Status.PostpondedId);
        }
    }
}