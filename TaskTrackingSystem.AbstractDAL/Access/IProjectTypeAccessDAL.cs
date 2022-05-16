using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.AbstractDAL.Access
{
    public interface IProjectTypeAccessDAL
    {
        List<ProjectTypeAccess> Get(string accountLogin);
        List<ProjectTypeAccess> GetAll();
        bool Delete(int id);
        bool Edit(ProjectTypeAccess projectTypeAccess);
        int Add(ProjectTypeAccess projectTypeAccess);
        void UpdateCache();
        void ClearCache();
    }
}
