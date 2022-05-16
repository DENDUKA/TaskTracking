using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface IAccountDAL
    {
        Account Get(string login);
        List<Account> GetAll();
        void ClearCache();
        int Create(Account account);
        void Delete(string login);
        bool Update(Account account);
    }
}