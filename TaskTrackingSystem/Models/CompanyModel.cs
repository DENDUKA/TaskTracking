using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class CompanyModel : IValidatableObject
    {
        public int Id { get; set; }
        [DisplayName("Название")]
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
        [DisplayName("Форма организации")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string Form { get; set; }

        public CompanyModel()
        {
        }

        public CompanyModel(int id, string name, string email, string phoneNumber, string form)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Form = form.ToString();
        }

        internal static List<CompanyModel> GetAll()
        {
            var companies = new List<CompanyModel>();

            foreach (var c in Logic.Logic.Instance.Company.GetAll())
            {
                companies.Add((CompanyModel)c);
            }

            return companies;
        }

        private List<ProjectModel> _projects;

        public List<ProjectModel> Projects
        {
            get
            {
                if (_projects is null)
                {
                    _projects = new List<ProjectModel>();
                    var c = Logic.Logic.Instance.Project.GetAllForCompany(Id);

                    foreach (var x in c)
                    {
                        _projects.Add((ProjectModel)x);
                    }
                }
                return _projects;
            }
        }

        private List<EmployeeModel> _employees;

        public List<EmployeeModel> Employees
        {
            get
            {
                if (_employees is null)
                {
                    _employees = new List<EmployeeModel>();
                    var e = Logic.Logic.Instance.Employee.GetAllForCompany(Id);

                    foreach (var x in e)
                    {
                        _employees.Add((EmployeeModel)x);
                    }
                }

                return _employees;
            }
        }

        private readonly IEnumerable<SelectListItem> _dropDownListForms = new List<SelectListItem>()
        {
            new SelectListItem(){ Text = "ИП", Value = "ИП"},
            new SelectListItem(){ Text = "ООО", Value = "ООО"},
            new SelectListItem(){ Text = "АО", Value = "АО"},
            new SelectListItem(){ Text = "ОАО", Value = "ОАО"},
            new SelectListItem(){ Text = "ПАО", Value = "ПАО"},
            new SelectListItem(){ Text = "ЗАО", Value = "ЗАО"},
            new SelectListItem(){ Text = "ФБУ", Value = "ФБУ"},
        };

        public IEnumerable<SelectListItem> DropDownListForms => _dropDownListForms;

        public static explicit operator CompanyModel(Company company)
        {
            return new CompanyModel(company.Id, company.Name, company.Email, company.PhoneNumber, company.Form);
        }
       
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Email != null && !Shared.Static.Regexes.EmailRegex.IsMatch(Email))
            {
                results.Add(new ValidationResult("Email - не валидный", new[] { "Email" }));
            }

            return results;
        }

        public static explicit operator Company(CompanyModel model)
        {
            return new Company(model.Id, model.Name, model.Email, model.PhoneNumber, model.Form);
        }
    }
}