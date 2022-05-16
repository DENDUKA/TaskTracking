using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.Logic;

namespace TaskTrackingSystem.Models
{
    public class TaskModel : IValidatableObject
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required(ErrorMessage = "Обязательное поле")]
        [MinLength(1)]
        [MaxLength(100, ErrorMessage = "Не больше 100 сиволов")]
        public string Name { get; set; }
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
        [DisplayName("Статус")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int StatusId { get; set; }
        [DisplayName("Статус")]
        public string StatusName { get; set; }
        [HiddenInput]
        public int ProjectId { get; set; }
        [DisplayName("Название проекта")]
        public string ProjectName { get; set; }
        [DisplayName("Исполнитель")]
        [Required(ErrorMessage = "Обязательное поле", AllowEmptyStrings = false)]
        public string AccountLogin { get; set; }
        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayName("Исполнитель")]
        public Account Account { get; set; }
        [DisplayName("Создал")]
        public Account Reporter { get; set; }
        [DisplayName("Оценка")]
        public int StoryPoints { get; set; }
        [DisplayName("Приватная")]
        public bool IsPrivate { get; set; }
        public bool IsApproved { get; set; }
        public bool ApproveAccess
        {
            get
            {
                if (Logic.Logic.Instance.AccountAccess.IsAdmin())
                {
                    return true;
                }

                if (Id != 0)
                {
                    var Task = Logic.Logic.Instance.Task.Get(Id);
                    this.ProjectId = Task.ProjectId;

                    if (Task.ReporterLogin == AccountBLL.CurrentUserLogin)
                    {
                        return true;
                    }
                }

                var project = ProjectBLL.Instance.Get(this.ProjectId);

                if (project == null)
                {
                    return false;
                }
                return project.ProjectAccess.Where(x => x.Responsible).Any(pa => pa.AccountLogin == AccountBLL.CurrentUserLogin);
            }
        }

        public double PercentTimeLeft => (this.DateEnd - DateTime.Now.Date).TotalSeconds / (this.DateEnd - this.DateStart).TotalSeconds;

        public string RowColor
        {
            get
            {
                // Добавить цвета у названий задач.
                // Если задача в процессе выполнения и срок до окончания более 20%, то цвет стандартный.
                // Если до срока сдачи осталось менее 20%, то цвет желтый.
                // Если задача просрочена, то цвет красный.
                // Если задача выполнена в срок и принята ответственным за проект, то цвет зеленый.
                if ((StatusId == Status.InWorkId || StatusId == Status.PostpondedId) && DateEnd < DateTime.Now.Date)
                {
                    return "#ff00001e";
                }
                if ((StatusId == Status.InWorkId || StatusId == Status.PostpondedId) && PercentTimeLeft <= 0.2D)
                {
                    return "#faff641e";//желтый
                }

                if (StatusId == Status.DoneId && IsApproved)
                {
                    return "#00ff001e";
                }

                return "white";
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (DateStart > DateEnd)
            {
                results.Add(new ValidationResult("Дата завершения, должна быть больше или равна дате начала", new[] { "DateEnd" }));
            }

            return results;
        }

        public List<ActionLog> Logs { get; private set; }

        public void FillLogs()
        {
            Logs = Logic.Logic.Instance.ActionLogs.Read(Id, "Task");
        }

        public List<CommentModel> Comments { get; private set; }

        public void FillComments()
        {
            Comments = new List<CommentModel>();
            
            var commentsDAL = Logic.Logic.Instance.Comment.GetForTask(Id).OrderByDescending(x => x.Time);
            foreach (var comment in commentsDAL)
            {
                Comments.Add((CommentModel)comment);
            }
        }

        /// <summary>
        /// Если текущий пользователь админ, то возвращаем вместе с удаленными тасками
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<TaskModel> GetTasksForProject(int projectId)
        {
            var tasks = Logic.Logic.Instance.Task.GetAllForProject(projectId, ModelHelper.IsCurrentUserAdmin());

            var model = new List<TaskModel>();
            foreach (var task in tasks)
            {
                model.Add((TaskModel)task);
            }

            return model;
        }

        public string GetColorForStatus => Logic.Logic.Instance.Status.GetColor(this.StatusName);

        public IEnumerable<SelectListItem> DropDownListStatuses => ModelHelper.GetDropDownListStatuses();

        public IEnumerable<SelectListItem> DropDownListAccounts => ModelHelper.GetDropDownListAccounts();

        public IEnumerable<SelectListItem> DropDownListPockerMarks => ModelHelper.GetDropDownListPockerMarks();

        public static explicit operator TaskModel(Task task)
        {
            return new TaskModel()
            {
                Id = task.Id,
                Name = task.Name,
                DateStart = task.DateStart,
                DateEnd = task.DateEnd,
                StatusId = task.Status is null ? 0 : task.Status.Id,
                StatusName = task.Status?.Name,
                ProjectId = task.ProjectId,
                ProjectName = task.Project.Name,
                Description = task.Description,
                Account = task.Account,
                AccountLogin = task.Account?.Login,
                Reporter = task.Reporter,
                StoryPoints = task.StoryPoints,
                IsPrivate = task.IsPrivate,
                IsApproved = task.IsApproved,
            };
        }

        public static explicit operator Task(TaskModel model)
        {
            var task =  new Task(model.Id, model.Name, model.DateStart, model.DateEnd, new Status(model.StatusId, ""), model.ProjectId, "", new Account("", model.AccountLogin), model.Description, model.Reporter, model.StoryPoints);
            task.AccountLogin = model.AccountLogin;
            task.StatusId = model.StatusId;
            task.IsPrivate = model.IsPrivate;
            task.IsApproved = model.IsApproved;
            return task;
        }

        #region AccessOld
        //public TaskAccess Access { get; private set; }

        //public void FillAccess(Task task, Project project)
        //{
        //    Access = Logic.Logic.Instance.Access.TaskAccess(task, project, ModelHelper.CurrentRole);
        //}

        //public void FillAccess(Task task, int projectId)
        //{
        //    var project = Logic.Logic.Instance.Project.Get(projectId);

        //    FillAccess(task, project);
        //}
        #endregion
    }
}