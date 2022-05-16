using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.UI.Web.Models
{
    public class AccountModel : IValidatableObject
    {
        [DisplayName("Логин")]
        [Required]
        public string Login { get; set; }
        [DisplayName("Имя")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Роль")]
        public string Role { get; set; }
        [DisplayName("Пароль")]
        [Required]
        public string Password { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Имя компьютера")]
        public string PCName { get; set; }
        [DisplayName("Скрыт")]
        public bool IsHidden { get; set; }
        [DisplayName("Должность")]
        public string Post { get; set; }
        [DisplayName("Заработная плата")]
        public float Wage { get; set; }
        [DisplayName("Отдел")]
        public string Department { get; set; }
        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateBirth { get; set; }
        [DisplayName("Образование")]
        public string Education { get; set; }
        [DisplayName("Обязанности")]
        public string Responsibilities { get; set; }

        public List<ProjectModel> Projects { get; private set; }
        public List<TaskModel> Tasks { get; private set; }
        public List<TimeTrackModel> TimeTrack { get; private set; }

        public IEnumerable<SelectListItem> RolesList => ModelHelper.GetDropDownListRoles();

        public AccountModel()
        {
            Projects = new List<ProjectModel>();
            Tasks = new List<TaskModel>();
            TimeTrack = new List<TimeTrackModel>();
        }

        internal static List<AccountModel> GetAll()
        {
            var accs = Logic.Logic.Instance.Account.GetAll();

            var models = new List<AccountModel>();
            foreach (var acc in accs)
            {
                models.Add((AccountModel)acc);
            }

            return models;
        }

        public static AccountModel Get(string login)
        {
            if (login == null)
                return null;

            var acc = Logic.Logic.Instance.Account.Get(login);

            if (acc == null)
                return null;

            var model = (AccountModel)acc;

            foreach (var tt in Logic.Logic.Instance.TimeTrack.GetAllForAccount(login))
            {
                model.TimeTrack.Add((TimeTrackModel)tt);
            }

            foreach (var task in Logic.Logic.Instance.Task.GetAllForAccount(login))
            {
                model.Tasks.Add((TaskModel)task);
            }

            var xxx = Logic.Logic.Instance.AccountAccess.GetAccessForAccount(login).ProjectAccesses;

            foreach (var pAccess in Logic.Logic.Instance.AccountAccess.GetAccessForAccount(login).ProjectAccesses)
            {
                if (pAccess.Responsible)
                {
                    var task = (ProjectModel)Logic.Logic.Instance.Project.Get(pAccess.ProjectId);
                    if (task.StatusId != Status.DeletedId)
                    {
                        model.Projects.Add(task);
                    }
                }
            }

            //foreach (var project in Logic.Logic.Instance.Project.GetAllForAccount(login))
            //{
            //    model.Projects.Add((ProjectModel)project);
            //}

            return model;
        }

        internal static bool LoginSuccess(string login, string password)
        {
            return Logic.Logic.Instance.Account.IsLoginSuccess(login, password);
        }

        internal static void Logout()
        {
            FormsAuthentication.SignOut();
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


        public static explicit operator Account(AccountModel model)
        {
            return new Account()
            {
                Name = model.Name,
                Login = model.Login,
                Password = model.Password,
                Role = model.Role,
                Email = model.Email,
                PCName = model.PCName,
                IsHidden = model.IsHidden,
                Post = model.Post,
                Wage = model.Wage,
                Department = model.Department,
                DateBirth = model.DateBirth,
                Education = model.Education,
                Responsibilities = model.Responsibilities,
            };
        }

        public static explicit operator AccountModel(Account acc)
        {
            return new AccountModel()
            {
                Login = acc.Login,
                Email = acc.Email,
                Name = acc.Name,
                Password = acc.Password,
                PCName = acc.PCName,
                Role = acc.Role,
                IsHidden = acc.IsHidden,
                Post = acc.Post,
                Wage = acc.Wage,
                Department = acc.Department,
                DateBirth = acc.DateBirth,
                Education = acc.Education,
                Responsibilities = acc.Responsibilities,
            };
        }
    }
}