using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class ProjectModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(100, ErrorMessage = "Не больше 100 сиволов")]
        [DisplayName("Название")]
        public string Name { get; set; }

        public ProjectType ProjectType { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DisplayName("Категория проектов")]
        public int ProjectTypeId { get; set; }

        [MaxLength(100, ErrorMessage = "Не больше 100 сиволов")]
        [DisplayName("Номер договора")]
        public string ContractNumber { get; set; }

        [DisplayName("Ответственный")]
        [Required(ErrorMessage = "Обязательное поле", AllowEmptyStrings = false)]
        public string AccountLogin { get; set; }

        [DisplayName("Заказчик")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [DisplayName("Ответственный")]
        public Account Account { get; set; }

        [DisplayName("Дата начала")]
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [DisplayName("Дата завершения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime DateEnd { get; set; }

        [DisplayName("Дата факт завершения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateEndFact { get; set; }

        [DisplayName("Статус")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int StatusId { get; set; }

        [DisplayName("Статус")]
        public Status Status { get; set; }

        [DisplayName("Статус договора")]
        public string ContractStatus { get; set; }

        [DisplayName("Стоимость")]
        [Range(-0.000001, 99999999999, ErrorMessage = "Введите не отрицательное число")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal Cost { get; set; }

        [DisplayName("Заплачено")]
        [Range(-0.000001, 99999999999, ErrorMessage = "Введите не отрицательное число")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal Paid { get; set; }

        [DisplayName("Премия выплачена")]
        public bool PremiumPaid { get; set; }

        public CalendarPlan CalendarPlan { get; set; }
        public string ResponsiblesString
        {
            get
            {
                var result = "";

                foreach (var x in Logic.Logic.Instance.AccountAccess.GetAccessForProject(Id).FindAll(x => x.Responsible is true))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += ",   ";
                    }
                    var name = Logic.Logic.Instance.Account.Get(x.AccountLogin).Name;

                    result += name;
                }

                return result;
            }
        }

        private List<Company> _companies = null;

        public List<Company> Companies
        {
            get
            {
                if (_companies is null)
                {
                    _companies = Logic.Logic.Instance.Company.GetAll().OrderBy(x => x.Name).ToList();
                }

                return _companies;
            }
        }

        //public ProjectAccess Access { get; private set; }

        public List<TaskModel> TaskList { get; set; }

        private List<Task> Tasks { get; set; }

        public List<ProjectPathToFile> FilePaths { get; set; }


        public void FillFilePaths()
        {
            FilePaths = Logic.Logic.Instance.Project.GetAllFilePaths(Id);
        }

        public static List<ProjectModel> GetAllWithPremiumUnPaid()
        {
            List<Project> projects = null;
            var models = new List<ProjectModel>();

            foreach (var pt in Logic.Logic.Instance.ProjectType.GetMainPTs())
            {
                projects = Logic.Logic.Instance.Project.GetAllForProjectType(pt.Id).Where(x => x.StatusId == Status.DoneId && !x.PremiumPaid).ToList();

                foreach (var p in projects)
                {
                    models.Add((ProjectModel)p);
                }
            }

            return models;
        }

        /// <summary>
        /// Если текущий пользователь админ, то получаем с удаленными тасками
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public void FillTaskList()
        {
            Tasks = Logic.Logic.Instance.Task.GetAllForProject(this.Id, ModelHelper.IsCurrentUserAdmin());

            var taskModels = new List<TaskModel>();
            foreach (var task in Tasks)
            {
                taskModels.Add((TaskModel)task);
            }

            //Сортировка "В работе" => "Отложено" => "Сделано" => "Удалено"
            this.TaskList = new List<TaskModel>();
            this.TaskList = this.TaskList.Concat(taskModels.FindAll(x => x.StatusId.Equals(Status.InWorkId)))
                .Concat(taskModels.FindAll(x => x.StatusId.Equals(Status.PostpondedId))
                .Concat(taskModels.FindAll(x => x.StatusId.Equals(Status.DoneId))
                .Concat(taskModels.FindAll(x => x.StatusId.Equals(Status.DeletedId)))))
                .ToList();


            FillPoints();
        }

        private void FillPoints()
        {
            AllPoints = TaskList.FindAll(x => x.StatusId != Status.DeletedId).Sum(x => x.StoryPoints);
            DonePoints = TaskList.FindAll(x => x.StatusId == Status.DoneId).Sum(x => x.StoryPoints);
            PostpondedPoints = TaskList.FindAll(x => x.StatusId == Status.PostpondedId).Sum(x => x.StoryPoints);
            InWorkPoints = TaskList.FindAll(x => x.StatusId == Status.InWorkId).Sum(x => x.StoryPoints);
            WithoutPoints = TaskList.FindAll(x => x.StoryPoints == 0 && x.StatusId != Status.DeletedId).Count;
        }

        public float AllPoints { get; private set; }
        public int DonePoints { get; private set; }
        public int PostpondedPoints { get; private set; }
        public int InWorkPoints { get; private set; }
        public int WithoutPoints { get; private set; }
        public float DonePersent => AllPoints == 0 ? 0 : DonePoints / AllPoints * 100;
        public float PostpondedPersent => AllPoints == 0 ? 0 : PostpondedPoints / AllPoints * 100;
        public float InWorkPersent => AllPoints == 0 ? 0 : InWorkPoints / AllPoints * 100;



        public string GetColorForStatus => Logic.Logic.Instance.Status.GetColor(this.Status.Name);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (DateStart > DateEnd)
            {
                results.Add(new ValidationResult("Дата завершения, должна быть больше или равна дате начала", new[] { "DateEnd" }));
            }

            if (Companies.Where(c => c.Id == CompanyId).Count() == 0)
            {
                results.Add(new ValidationResult("Обязательное поле", new[] { "CompanyId" }));
            }

            return results;
        }

        public IEnumerable<SelectListItem> DropDownListAccounts => ModelHelper.GetDropDownListAccounts();

        public IEnumerable<SelectListItem> DropDownListStatuses => ModelHelper.GetDropDownListStatuses();

        public IEnumerable<SelectListItem> DropDownListContractStatuses => ModelHelper.GetDropDownListContractStatuses(ContractStatus);

        public IEnumerable<SelectListItem> DropDownListProjectType => ModelHelper.GetDropDownListProjectType(ProjectTypeId);        

        public static List<ProjectModel> SortByEndDate(List<ProjectModel> models)
        {
            return models.OrderBy(m => m.DateEnd).ToList();
        }

        internal static List<ProjectModel> GetAllForProjectType(int id)
        {
            var projects = Logic.Logic.Instance.Project.GetAllForProjectType(id);

            var models = new List<ProjectModel>();

            foreach (var p in projects)
            {
                models.Add((ProjectModel)p);
            }

            return models;
        }


        public List<ActionLog> Logs { get; private set; }
        

        public void FillLogs()
        {
            Logs = Logic.Logic.Instance.ActionLogs.Read(Id, "Project");
        }

        public static explicit operator ProjectModel(Project project)
        {
            return new ProjectModel()
            {
                Id = project.Id,
                Name = project.Name,
                ProjectType = project.ProjectType,
                ProjectTypeId = project.ProjectType.Id,
                Account = project.Account,
                AccountLogin = project.Account?.Login,
                DateStart = project.DateStart,
                DateEnd = project.DateEnd,
                DateEndFact = project.DateEndFact,
                Status = project.Status,
                ContractStatus = project.ContractStatus,
                StatusId = project.Status.Id,
                ContractNumber = project.ContractNumber,
                Cost = project.Cost,
                Paid = project.Paid,
                CompanyId = project.CompanyId,
                Company = project.Company,
                PremiumPaid = project.PremiumPaid,
            };
        }


        public static explicit operator Project(ProjectModel model)
        {

            var p = new Project(model.Id, model.Name, new Account(model.Account?.Name, model.AccountLogin), new ProjectType(model.ProjectTypeId, model.ProjectType?.Name),
                new Status(model.StatusId, Status.All[model.StatusId].Name),
                model.ContractStatus, model.DateStart, model.DateEnd, model.DateEndFact, model.ContractNumber, model.Cost, model.Paid, model.CompanyId, model.PremiumPaid);

            p.Responsible = p.Account.Login;
            p.ProjectTypeId = p.ProjectType.Id;
            p.StatusId = p.Status.Id;


            return p;
        }
    }
}