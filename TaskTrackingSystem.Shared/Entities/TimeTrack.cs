using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("TimeTrack")]
    public class TimeTrack
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [ForeignKey("Login")]
        public virtual Account Account { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        [Required]
        public string Description { get; set; }

        public static List<string> ActionPlusTime = new List<string>() { "Опоздание", "Отгул" };
        public static List<string> ActionMinusTime = new List<string>() { "Отработка" };

        public TimeTrack(int id, Account acc, string type, DateTime dateStart, DateTime dateEnd, string description)
        {
            this.Id = id;
            this.Account = acc;
            this.Type = type;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.Description = description;
            this.Login = acc?.Login;
        }

        public TimeTrack()
        { }
    }
}