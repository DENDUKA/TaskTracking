using System.Collections.Generic;
using System.Web;
using System.Linq;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Access
{
    public class ProjectTypeAccess
    {
        private readonly int projectTypeId;
        private readonly string currentRole;        
        private string Login => HttpContext.Current.User.Identity.Name;

        public Dictionary<int, ProjectAccess> AccessToProjects = new Dictionary<int, ProjectAccess>();

        public ProjectTypeAccess(int projectTypeid, List<Project> projectList)
        {
            this.projectTypeId = projectTypeid;

            currentRole = Logic.Instance.Account.GetRole(Login);

            AccessToCreateNew();
            GetAccessForProjects(projectList);
            AccessToColumnEdit();
            AccessToColumnCost();
            AccessToExelReport();
        }

        public bool CreateNewProject { get; private set; }
        public bool ColumnEdit { get; private set; }
        public bool ColumnCost { get; private set; }
        public bool ExelReport { get; private set; }

        private void AccessToColumnEdit()
        {
            ColumnEdit = AccessToProjects.Any(x => x.Value.Edit);
        }

        private void AccessToExelReport()
        {
            ExelReport = Role.AdmiBuhList.Contains(currentRole);
        }

        private void AccessToColumnCost()
        {
            ColumnCost = AccessToProjects.Any(x=>x.Value.CostField);
        }

        private void GetAccessForProjects(List<Project> projectList)
        {
            foreach (var p in projectList)
            {
                AccessToProjects.Add(p.Id, new ProjectAccess(p, currentRole));
            }
        }

        private void AccessToCreateNew()
        {
            CreateNewProject =
                Role.AdmiBuhList.Contains(currentRole) || //Если админ или бухгалтер
                Role.ProjectTypeLeads[projectTypeId].Equals(currentRole); //Если ответственный за Категорию проектов
        }
    }
}
