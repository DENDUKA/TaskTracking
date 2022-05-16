using System;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
        public Account Account { get; set; }
        public int TaskId { get; set; }

        public static explicit operator CommentModel(Comment comment)
        {
            return new CommentModel()
            {
                Id = comment.Id,
                Time = comment.Time,
                Text = comment.Text,
                Account = Logic.Logic.Instance.Account.Get(comment.Login),
                TaskId = comment.TaskId,
            };
        }

        public static explicit operator Comment(CommentModel model)
        {
            return new Comment(model.Id, model.TaskId, model.Account.Login, model.Time, model.Text);
        }
    }
}