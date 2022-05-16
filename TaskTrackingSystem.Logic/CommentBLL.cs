using System;
using System.Collections.Generic;
using System.Web;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class CommentBLL
    {
        #region singleton
        public static CommentBLL Instance { get; private set; }

        private readonly ICommentDAL commentDAL;

        private CommentBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    commentDAL = new CommentDAL();
                    break;
                case "EFDB":
                    commentDAL = new EFDAL.CommentDAL();
                    break;
            }


            commentDAL = new CommentDAL();
        }

        static CommentBLL()
        {
            Instance = new CommentBLL();
        }
        #endregion

        public Comment Get(int id)
        {
            return commentDAL.Get(id);
        }
        public int Add(Comment comment)
        {
            comment.Login = HttpContext.Current.User.Identity.Name;
            comment.Time = DateTime.Now;
            return commentDAL.AddForTask(comment);
        }
        public List<Comment> GetForTask(int taskId)
        {
            return commentDAL.GetForTask(taskId);
            
        }
        public OperationResult Delete(int id)
        {
            return commentDAL.Delete(id);
        }
    }
}