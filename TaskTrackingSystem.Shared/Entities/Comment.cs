using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Comment")]
    public class Comment
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual Task Task { get; set; }

        [StringLength(100)]
        public string Login { get; set; }

        [ForeignKey("Login")]
        public virtual Account Account { get; set; }

        public DateTime Time { get; set; }

        public string Text { get; set; }

        public Comment(int id, int taskId, string login, DateTime time, string text)
        {
            this.Id = id;
            this.TaskId = taskId;
            this.Login = login;
            this.Time = time;
            this.Text = text;
        }

        private Comment()
        { }
    }
}
