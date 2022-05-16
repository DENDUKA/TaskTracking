using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Status")]
    public class Status : ICloneable
    {
        public Status(int id, string name)
        {
            Id = id;
            Name = name;
        }
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual List<Project> Project { get; set; }

        public virtual List<Task> Task { get; set; }

        public const int PostpondedId = 1;
        public const int InWorkId = 2;
        public const int DoneId = 3;
        public const int DeletedId = 4;

        public const string Postponded = "Отложена";
        public const string InWork = "В работе";
        public const string Done = "Завершена";
        public const string Deleted = "Удалена";

        public static readonly Dictionary<int, Status> All = new Dictionary<int, Status>()
        {
            { InWorkId , new Status(InWorkId, InWork )},
            { PostpondedId , new Status(PostpondedId, Postponded)},            
            { DoneId , new Status(DoneId, Done) },
            { DeletedId , new Status(DeletedId, Deleted )},
        };

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private Status()
        { }
    }
}