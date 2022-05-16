using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.AbstractDAL.Access
{
    public interface IProjectAccessDAL
    {
        List<ProjectAccess> Get(string accountLogin);
        List<ProjectAccess> GetAll();
        bool Delete(int id);
        bool Edit(ProjectAccess projectTypeAccess);
        int Add(ProjectAccess projectTypeAccess);
        void UpdateCache();
        void ClearCache();
    }
}
