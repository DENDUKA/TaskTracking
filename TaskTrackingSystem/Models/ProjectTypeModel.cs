using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class ProjectTypeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(50, ErrorMessage = "Не больше 50 сиволов")]
        [DisplayName("Название")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExcelStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExcelEnd { get; set; }

        public List<ProjectModel> ProjectList { get; set; }

        public static List<ProjectTypeModel> GetAll()
        {
            var model = new List<ProjectTypeModel>();

            var devLogins = Logic.Logic.Instance.Account.GetAll().Where(u => u.Role == Role.Dev).Select(u=>u.Login);
            var devProjectType = new string[] { "Разработка ПО" };
            var login = HttpContext.Current.User.Identity.Name;
            var role = Logic.Logic.Instance.Account.Get(login).Role;

            foreach (var item in Logic.Logic.Instance.ProjectType.GetAll())
            {
                if (role == Role.Admin)
                {
                    model.Add(new ProjectTypeModel() { Id = item.Id, Name = item.Name });
                }
                else if (devLogins.Contains(login))
                {
                    if (devProjectType.Contains(item.Name))
                    {
                        model.Add(new ProjectTypeModel() { Id = item.Id, Name = item.Name });
                    }
                }
                else if (!devLogins.Contains(login))
                {
                    if (!devProjectType.Contains(item.Name))
                    {
                        model.Add(new ProjectTypeModel() { Id = item.Id, Name = item.Name });
                    }
                }
            }

            return model;
        }

        public void FillAccess()
        {
        }

        public static explicit operator ProjectTypeModel(ProjectType pt)
        {
            return new ProjectTypeModel
            {
                Id = pt.Id,
                Name = pt.Name
            };
        }

        private List<Project> projects = new List<Project>();

        public void FillProjectList()
        {
            projects = Logic.Logic.Instance.Project.GetAllForProjectType(this.Id);

            var projectModels = new List<ProjectModel>();

            foreach (var p in projects)
            {
                projectModels.Add((ProjectModel)p);
            }

            this.ProjectList = projectModels;
        }
    }
}