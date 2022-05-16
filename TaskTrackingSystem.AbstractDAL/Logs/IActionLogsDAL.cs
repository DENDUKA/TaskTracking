using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL.Logs
{
    public interface IActionLogsDAL
    {
        void Add(int taskId, string userLogin, string fieldName, string oldValue, string newValue, string type);
        List<ActionLog> Read(int taskId, string type);
    }
}
