using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Account")]
    public class Account : ICloneable
    {
        public Account(string name, string login)
        {
            Name = name;
            Login = login;
        }

        public Account(string name, string login, string password, string role, string email, string pcName, bool isHidden = false)
        {
            Name = name;
            Login = login;
            Password = password;
            Role = role;
            Email = email;
            PCName = pcName;
            IsHidden = isHidden;
        }

        public Account()
        {}        

        [Key]
        [StringLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(80)]
        public string Email { get; set; }

        [StringLength(80)]
        public string PCName { get; set; }

        public bool IsHidden { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        [StringLength(100)]
        public string Post { get; set; }
        /// <summary>
        /// Заработная плата
        /// </summary>
        public float Wage { get; set; }
        /// <summary>
        /// Отдел
        /// </summary>
        [StringLength(100)]        
        public string Department { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateBirth { get; set; }
        public string Education { get; set; }
        /// <summary>
        /// Обязанности
        /// </summary>
        public string Responsibilities { get; set; }

        public virtual List<AccountAccess> AccountAccess { get; set; }

        public virtual List<Comment> Comment { get; set; }

        public virtual List<Project> Project { get; set; }

        public virtual List<ProjectAccess> ProjectAccess { get; set; }

        public virtual List<ProjectTypeAccess> ProjectTypeAccess { get; set; }

        public virtual List<TimeTrack> TimeTrack { get; set; }

        [ForeignKey("AccountLogin")]
        public virtual List<Task> Task { get; set; }

        [ForeignKey("ReporterLogin")]
        public virtual List<Task> AsReporter { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}