using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface IProjectDAL
    {
        Project Get(int id);
        List<Project> GetAllForProjectType(int projTypeId);
        List<Project> GetAllForCompany(int companyId);
        List<Project> GetAllForAccount(string login);
        bool Update(Project project);
        OperationResult UpdateStatus(int id, int statusId);
        OperationResult Delete(int id);
        int Create(Project project);
        List<ProjectPathToFile> GetFilePaths(int projectId);
        int CreateFilePath(int projectId);
        OperationResult DeleteFilePath(int filePathId);
        OperationResult UpdateFilePath(ProjectPathToFile filePath);
        void ChangeProjectType(int projectId, int projectTypeId);
    }
}
