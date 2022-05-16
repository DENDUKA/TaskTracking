using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class ProjectLocalDAL : IProjectDAL
    {
        public void ChangeProjectType(int projectId, int projectTypeId)
        {
            throw new System.NotImplementedException();
        }

        public int Create(Project project)
        {
            project.Id = LocalStorage.Projects.Keys.Max() + 1;
            project.ProjectType = LocalStorage.ProjectTypes[project.ProjectType.Id];


            LocalStorage.Projects.Add(project.Id, project);
            return project.Id;
        }

        public int CreateFilePath(int projectId)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Delete(int id)
        {
           return new OperationResult(LocalStorage.Projects.Remove(id));
        }

        public OperationResult DeleteFilePath(int filePathId)
        {
            throw new System.NotImplementedException();
        }

        public Project Get(int id)
        {
            return LocalStorage.Projects[id];
        }

        public List<Project> GetAllForAccount(string login)
        {
            return LocalStorage.Projects.Values.ToList();
        }

        public List<Project> GetAllForCompany(int companyId)
        {
            return LocalStorage.Projects.Values.ToList().FindAll(x => x.CompanyId == companyId);
        }

        public List<Project> GetAllForProjectType(int projTypeId)
        {
            return LocalStorage.Projects.Values.ToList().FindAll(x => x.ProjectType.Id == projTypeId);
        }

        public List<ProjectPathToFile> GetFilePaths(int projectId)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Project project)
        {
            LocalStorage.Projects[project.Id].Name = project.Name;
            LocalStorage.Projects[project.Id].Account = LocalStorage.Accounts[project.Account.Login];
            LocalStorage.Projects[project.Id].DateStart = project.DateStart;
            LocalStorage.Projects[project.Id].DateEnd = project.DateEnd;
            LocalStorage.Projects[project.Id].Status = LocalStorage.Statuses[project.Status.Id];
            LocalStorage.Projects[project.Id].ContractNumber = project.ContractNumber;
            LocalStorage.Projects[project.Id].CompanyId = project.CompanyId;
            LocalStorage.Projects[project.Id].Company = LocalStorage.Companies[project.CompanyId];
            LocalStorage.Projects[project.Id].Cost = project.Cost;
        }

        public OperationResult UpdateFilePath(ProjectPathToFile filePath)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult UpdateStatus(int id, int statusId)
        {
            LocalStorage.Projects[id].Status = LocalStorage.Statuses[statusId];
            return new OperationResult(true);
        }

        bool IProjectDAL.Update(Project project)
        {
            throw new System.NotImplementedException();
        }
    }
}
