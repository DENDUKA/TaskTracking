using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class ProjectDAL : IProjectDAL
    {
        public Project Get(int id)
        {
            using (var db = new ProjectContext())
            {
                var ps = db.Projects
                    .Where(x => x.Id == id)
                    .Include(x => x.Account)
                    .Include(x => x.Company)
                    .Include(x => x.ProjectType)
                    .Include(x => x.Status)
                    .Include(x => x.Task)
                    .Include(x => x.ProjectAccess)
                    .FirstOrDefault();

                return ps;
            }
        }

        public List<Project> GetAllForProjectType(int projTypeId)
        {
            using (var db = new ProjectContext())
            {
                return db.Projects
                    .Where(x => x.ProjectTypeId == projTypeId)
                    .Include(x => x.Account)
                    .Include(x => x.Company)
                    .Include(x => x.ProjectType)
                    .Include(x => x.Status)
                    .ToList();
            }
        }

        public List<Project> GetAllForCompany(int companyId)
        {
            using (var db = new ProjectContext())
            {
                return db.Projects
                    .Where(x => x.CompanyId == companyId)
                    .Include(x => x.Account)
                    .Include(x => x.Company)
                    .Include(x => x.ProjectType)
                    .Include(x => x.Status)
                    .ToList();
            }
        }

        public int Create(Project project)
        {
            using (var db = new ProjectContext())
            {
                if (project.Account != null)
                    db.Entry(project.Account).State = EntityState.Unchanged;

                if (project.Company != null)
                    db.Entry(project.Company).State = EntityState.Unchanged;

                if (project.ProjectType != null)
                    db.Entry(project.ProjectType).State = EntityState.Unchanged;

                if (project.Status != null)
                    db.Entry(project.Status).State = EntityState.Unchanged;

                if (project.ContractStatus == null)
                    project.ContractStatus = Project.DefaultContractStatus;

                db.Projects.Add(project);
                db.SaveChanges();
            }

            return project.Id;
        }

        public List<Project> GetAllForAccount(string login)
        {
            using (var db = new ProjectContext())
            {
                return db.Projects
                    .Where(x => x.Responsible == login)
                    .ToList();
            }
        }

        public OperationResult UpdateStatus(int id, int statusId)
        {
            using (var db = new ProjectContext())
            {
                var project = db.Projects
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (project != null)
                {
                    project.StatusId = statusId;

                    return new OperationResult(db.SaveChanges() > 0);
                }

                return new OperationResult(false);
            }
        }

        public OperationResult Delete(int id)
        {
            using (var db = new ProjectContext())
            {
                var res = db.Projects
                     .Where(x => x.Id == id)
                     .Delete();
                return new OperationResult(res > 0);
            }
        }

        public bool Update(Project project)
        {
            using (var db = new ProjectContext())
            {
                project.Account = null;

                if (project.Cost == 0)
                {
                    project.Cost = Get(project.Id).Cost;
                }

                try
                {
                    db.Projects.AddOrUpdate(project);
                    return db.SaveChanges() > 0;
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }

        public List<ProjectPathToFile> GetFilePaths(int id)
        {
            using (var db = new ProjectContext())
            {
                return db.Projects
                    .Where(x => x.Id == id)
                    .Include(x => x.ProjectPathToFile)
                    .FirstOrDefault()?.ProjectPathToFile.ToList();
            }
        }

        public int CreateFilePath(int projectId)
        {
            var ppf = new ProjectPathToFile(0, projectId, "", "");

            using (var db = new ProjectPathToFileContext())
            {
                db.ProjectPathToFiles.Add(ppf);

                db.SaveChanges();
            }

            return ppf.Id;
        }

        public OperationResult DeleteFilePath(int filePathId)
        {
            using (var db = new ProjectPathToFileContext())
            {
                var res = db.ProjectPathToFiles
                     .Where(x => x.Id == filePathId)
                     .Delete();
                return new OperationResult(res > 0);
            }
        }

        public OperationResult UpdateFilePath(ProjectPathToFile filePath)
        {
            using (var db = new ProjectPathToFileContext())
            {
                db.ProjectPathToFiles.Attach(filePath);

                db.Entry(filePath).Property(x => x.Description).IsModified = true;
                db.Entry(filePath).Property(x => x.Path).IsModified = true;

                return new OperationResult(db.SaveChanges() > 0);
            }
        }

        public void ChangeProjectType(int projectId, int projectTypeId)
        {
            using (var db = new ProjectContext())
            {
                var project = db.Projects.Where(x => x.Id == projectId).FirstOrDefault();

                if (project != null)
                {
                    project.ProjectTypeId = projectTypeId;

                    db.SaveChanges();
                }
            }
        }
    }
}