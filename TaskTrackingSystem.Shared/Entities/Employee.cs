using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public int? AdditionalNumber { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }


        private Employee()
        { }

        public Employee(int id, string name, string phoneNumber, int? additionalNumber, string email, int companyId)
        {
            this.Id = id;
            this.CompanyId = id;
            this.Name = name;
            this.PhoneNumber = phoneNumber;
            this.AdditionalNumber = additionalNumber;
            this.Email = email;
            this.CompanyId = companyId;
        }
    }
}