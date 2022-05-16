using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class CommentDAL : ICommentDAL
    {
        public int AddForTask(Comment comment)
        {
            using (var db = new CommentContext())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return comment.Id;
            }
        }

        public OperationResult Delete(int id)
        {
            using (var db = new CommentContext())
            {
                var res = db.Comments
                    .Where(x => x.Id == id)
                    .Delete();

                return new OperationResult(res > 0);
            }
        }

        public Comment Get(int id)
        {
            using (var db = new CommentContext())
            {
                return db.Comments
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        public List<Comment> GetForTask(int taskId)
        {
            using (var db = new CommentContext())
            {
                return db.Comments
                    .Where(x => x.TaskId == taskId)
                    .ToList();
            }
        }
    }
}