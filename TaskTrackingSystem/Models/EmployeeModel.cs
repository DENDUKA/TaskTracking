using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class EmployeeModel : IValidatableObject
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(200, ErrorMessage = "Не больше 200 сиволов")]
        public string Name { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Телефон")]
        [MaxLength(10, ErrorMessage = "Количество цифр должно быть 10")]
        [MinLength(10, ErrorMessage = "Количество цифр должно быть 10")]
        [Range(-1, 99999999999, ErrorMessage = "Только числа")]
        [DataType(DataType.Currency)]
        public string PhoneNumber { get; set; }

        public string DisplayPhoneNumber => string.IsNullOrEmpty(PhoneNumber) ? "" : $"+7 {PhoneNumber}";

        [DisplayName("Добавочный номер")]
        [Range(-1, 99999999999, ErrorMessage = "Только числа")]
        public int? AddititionalNumber { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public EmployeeModel()
        {
        }

        public EmployeeModel(int id, string name, string email, string phoneNumber, int? addititionalNumber, int companyId)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.AddititionalNumber = addititionalNumber;
            this.CompanyId = companyId;
        }

        public static explicit operator EmployeeModel(Employee employee)
        {
            return new EmployeeModel(employee.Id, employee.Name, employee.Email, employee.PhoneNumber, employee.AdditionalNumber, employee.CompanyId);
        }

        public static explicit operator Employee(EmployeeModel model)
        {
            return new Employee(model.Id, model.Name, model.PhoneNumber, model.AddititionalNumber, model.Email, model.CompanyId);
        }


        private static readonly Regex EmailRegex = new Regex(@"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}");
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Email != null && !EmailRegex.IsMatch(Email))
            {
                results.Add(new ValidationResult("Email - не валидный", new[] { "Email" }));
            }

            return results;
        }
    }
}