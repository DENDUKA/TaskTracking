using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using AccountEF = TaskTrackingSystem.DAL.EFDAL.EFBD.Account;
using Account = TaskTrackingSystem.Shared.Entities.Account;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL
{
    public class AccountEFDAL : IAccountDAL
    {
        public void ClearCache()
        {
            //throw new NotImplementedException();
        }

        public int Create(Account account)
        {
            Shared.Context.Account.Add(account.ToEF());
            Shared.Context.SaveChanges();
            return 0;
        }

        public void Delete(string login)
        {
            var account = Shared.Context.Account.FirstOrDefault(a => a.Login == login);

            if (account != null)
            {
                Shared.Context.Account.Remove(account);
                Shared.Context.SaveChanges();
            }
        }

        public Account Get(string login)
        {
            var acc = Shared.Context.Account.FirstOrDefault(a => a.Login == login);
            var some = Shared.Context.Company.ToList();

            return acc?.ToAccount();
        }

        public List<Account> GetAll()
        {
            var listAccs = new List<Account>();
            foreach (var a in Shared.Context.Account.ToList())
            {
                listAccs.Add(a.ToAccount());
            }
            return listAccs;
        }

        public bool Update(Account account)
        {
            throw new System.NotImplementedException();
        }
    }
}
