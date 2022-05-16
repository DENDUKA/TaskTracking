using TaskTrackingSystem.DAL;
using TaskTrackingSystem.AbstractDAL;
using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;
using System.Linq;

namespace TaskTrackingSystem.Logic
{
    public class ProjectTypeBLL
    {
        #region singleton
        public static ProjectTypeBLL Instance { get; private set; }

        private readonly IProjectTypeDAL projectTypeDAL;

        private ProjectTypeBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    projectTypeDAL = new ProjectTypeDAL();
                    break;
                case "Local":
                    projectTypeDAL = new ProjectTypeLocalDAL();
                    break;
                case "EFDB":
                    projectTypeDAL = new EFDAL.ProjectTypeDAL();
                    break;
            }
        }

        static ProjectTypeBLL()
        {
            Instance = new ProjectTypeBLL();
        }
        #endregion

        public List<ProjectType> GetAll()
        {
            return projectTypeDAL.GetAll();
        }

        public List<ProjectType> GetMainPTs()
        {
            return projectTypeDAL.GetAll().Where(x => x.Id >= 1 && x.Id <= 4).ToList();
        }

        public ProjectType Get(int id)
        {
            return projectTypeDAL.Get(id);
        }

        public string GetProjectTypeName(string id)
        {
            if (int.TryParse(id, out var _id))
            {
                var pt = Get(_id);
                if (pt != null)
                    return pt.Name;
            }

            return "";
        }
    }
}