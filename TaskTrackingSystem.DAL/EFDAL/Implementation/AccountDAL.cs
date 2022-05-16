using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class AccountDAL : IAccountDAL
    {
        private Dictionary<string, Account> accountCache = new Dictionary<string, Account>();
        private bool getAll = false;

        public Account Get(string login)
        {
            if (accountCache.ContainsKey(login))
            {
                return (Account)accountCache[login].Clone();
            }

            var account = GetFromDB(login);

            if (account is null)
                return null;

            accountCache.Add(login, account);

            return (Account)account.Clone();
        }

        public Account GetFromDB(string login)
        {
            using (var db = new AccountContext())
            {
                var acc = db.Accounts
                    .Where(a => a.Login == login)
                    .FirstOrDefault();
                return acc;
            }
        }

        public List<Account> GetAll()
        {
            if (!getAll)
            {
                accountCache.Clear();

                foreach (var a in GetAllFromDB())
                {
                    accountCache.Add(a.Login, a);
                }

                getAll = true;
            }

            return accountCache.Select(status => (Account)status.Value.Clone()).ToList();
        }

        private List<Account> GetAllFromDB()
        {
            using (var db = new AccountContext())
            {
                var accs = db.Accounts
                    .ToList();

                return accs;
            }
        }

        public void ClearCache()
        {
            accountCache.Clear();
            getAll = false;
        }

        public int Create(Account account)
        {
            ClearCache();

            using (var db = new AccountContext())
            {
                db.Accounts.Add(account);
                return db.SaveChanges() > 0 ? 1 : 0;
            }
        }

        public void Delete(string login)
        {
            ClearCache();

            using (var db = new AccountContext())
            {
                db.Accounts
                    .Where(x => x.Login == login)
                    .Delete();
            }
        }

        public bool Update(Account acc)
        {
            ClearCache();

            using (var db = new AccountContext())
            {
                db.Accounts.AddOrUpdate(acc);
                db.SaveChanges();
            }

            return true;
        }
    }
}