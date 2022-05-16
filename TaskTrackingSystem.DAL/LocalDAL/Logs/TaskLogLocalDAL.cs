using System;
using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL.Logs;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.ActionLogs
{
    public class TaskLogLocalDAL : IActionLogsDAL
    {
        public void Add(int taskId, string userLogin, string fieldName, string oldValue, string newValue)
        {
           
        }

        public void Add(int taskId, string userLogin, string fieldName, string oldValue, string newValue, string type)
        {
            throw new NotImplementedException();
        }

        public List<ActionLog> Read(int taskId)
        {
            return new List<ActionLog>();
        }

        public List<ActionLog> Read(int taskId, string type)
        {
            throw new NotImplementedException();
        }
    }
}
