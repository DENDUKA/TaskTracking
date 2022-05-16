using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ICommentDAL
    {
        Comment Get(int id);
        int AddForTask(Comment comment);
        List<Comment> GetForTask(int taskId);        
        OperationResult Delete(int id);
    }
}
