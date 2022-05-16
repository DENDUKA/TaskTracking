using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.AbstractDAL.Access
{
    public interface IUserAccessDAL
    {
        AccountAccess Get(string accountLogin, int projectId = 0);
        List<AccountAccess> GetAll();
        void UpdateCache();
        bool Add(AccountAccess accountAccess);
        bool Edit(AccountAccess accountAccess);
        bool Delete(string accountLogin);
        void ClearCache();
    }
}
