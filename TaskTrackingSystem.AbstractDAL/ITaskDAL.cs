using System;
using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ITaskDAL
    {
        Task Get(int id);
        List<Task> GetAllForProject(int projectId);
        int Add(Task task);
        OperationResult Delete(int id);
        OperationResult UpdateStatus(int id, int statusId);
        bool UpdateMainFiels(Task task);
        List<Task> GetAllForAccount(string login);
        List<Task> GetAll(DateTime start, DateTime end, int[] statusId);
    }
}
