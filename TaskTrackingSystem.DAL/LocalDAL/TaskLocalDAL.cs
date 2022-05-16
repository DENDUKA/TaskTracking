using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class TaskLocalDAL : ITaskDAL
    {
        public int Add(Task task)
        {
            var id = LocalStorage.Tasks.Keys.Max() + 1;
            LocalStorage.Tasks.Add(id, new Task(id, task.Name, task.DateStart, task.DateEnd, LocalStorage.Statuses[task.Status.Id], task.ProjectId,
               LocalStorage.Projects[task.ProjectId].Name, LocalStorage.Accounts[task.Account.Login], task.Description, task.Reporter, task.StoryPoints));
            return id;
        }

        public OperationResult Delete(int id)
        {
            LocalStorage.Tasks.Remove(id);
            return new OperationResult(true);
        }

        public Task Get(int id)
        {
            return LocalStorage.Tasks[id];
        }

        public List<Task> GetAll(System.DateTime start, System.DateTime end, int[] statusId)
        {
            throw new System.NotImplementedException();
        }

        public List<Task> GetAllForAccount(string login)
        {
            return LocalStorage.Tasks.Values.ToList();
        }

        public List<Task> GetAllForProject(int projectId)
        {
            return LocalStorage.Tasks.Values.ToList().FindAll(x => x.ProjectId == projectId);
        }

        public bool UpdateMainFiels(Task task)
        {
            var projectid = LocalStorage.Tasks[task.Id].ProjectId;

            LocalStorage.Tasks[task.Id] = new Task(task.Id, task.Name, task.DateStart, task.DateEnd, LocalStorage.Statuses[task.Status.Id], projectid,
               LocalStorage.Projects[projectid].Name, LocalStorage.Accounts[task.Account.Login], task.Description, task.Reporter, task.StoryPoints);

            return true;
        }

        public OperationResult UpdateStatus(int id, int statusId)
        {
            LocalStorage.Tasks[id].Status = LocalStorage.Statuses[statusId];
            return new OperationResult(true);
        }
    }
}