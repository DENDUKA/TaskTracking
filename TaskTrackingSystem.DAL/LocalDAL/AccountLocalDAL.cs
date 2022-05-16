using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class AccountLocalDAL : IAccountDAL
    {
        public void ClearCache()
        {

        }

        public int Create(Account account)
        {
            LocalStorage.Accounts.Add(account.Login, account);
            return 0;
        }

        public void Delete(string login)
        {
            LocalStorage.Accounts.Remove(login);
        }

        public Account Get(string login)
        {
            if (!LocalStorage.Accounts.ContainsKey(login))
                return null;

            return LocalStorage.Accounts[login];
        }

        public List<Account> GetAll()
        {
            return LocalStorage.Accounts.Values.ToList();
        }

        public bool Update(Account account)
        {
            throw new System.NotImplementedException();
        }
    }
}