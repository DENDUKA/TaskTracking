using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Company")]
    public class Company : ICloneable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Form { get; set; }

        public virtual List<Employee> Employee { get; set; }

        public virtual List<Project> Project { get; set; }
        

        private Company()
        { }

        public Company(int id , string name, string email, string phoneNumber, string form)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Form = form;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static List<string> CompanyForms = new List<string>() { "ИП", "ООО", "АО", "ПАО", "ЗАО" };
    }
}
