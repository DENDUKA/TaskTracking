using System.Collections.Generic;
using System.Web;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class AccountBLL
    {
        #region singleton
        public static AccountBLL Instance { get; private set; }

        private readonly IAccountDAL accountDAL;

        private AccountBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "EFDB":
                    accountDAL = new EFDAL.AccountDAL(); //new AccountDAL();
                    break;
                case "DB":
                    accountDAL = new AccountDAL(); //new AccountDAL();
                    break;
                case "Local":
                    accountDAL = new AccountLocalDAL();
                    break;
            }
        }

        static AccountBLL()
        {
            Instance = new AccountBLL();
        }
        #endregion

        public static string CurrentUserLogin => HttpContext.Current.User.Identity.Name;

        public bool IsLoginSuccess(string login, string password)
        {
            var acc = accountDAL.Get(login);
            if (acc != null)
            {
                return !acc.IsHidden && acc.Password.Equals(password);
            }
            return false;
        }

        public List<Account> GetAll(bool getIsHidden = false)
        {

            var accounts = accountDAL.GetAll();

            if (!getIsHidden)
            {
                accounts = accountDAL.GetAll().FindAll(acc => acc.IsHidden == false);
            }

            return accounts;
        }

        public int Create(Account account)
        {
            if (account.Email != null && !Shared.Static.Regexes.EmailRegex.IsMatch(account.Email))
                return 0;

            var id =  accountDAL.Create(account);

            Logic.Instance.Cache.Clear();

            return id;
        }

        public bool Update(Account account)
        {
            return accountDAL.Update(account);
        }

        public Account Get(string login)
        {
            return accountDAL.Get(login);
        }

        public string[] GetRoles(string login)
        {
            var user = Get(login);

            if (user != null)
            {
                switch (user.Role)
                {
                    case Role.Admin:
                        return Role.AdminAccess;
                    case Role.Buh:
                        return Role.BuhAccess;
                    case Role.Lead1:
                        return Role.LeadAccess;
                    case Role.Lead2:
                        return Role.LeadAccess;
                    case Role.Lead3:
                        return Role.LeadAccess;
                    case Role.Lead4:
                        return Role.LeadAccess;
                    case Role.Lead5:
                        return Role.LeadAccess;
                    case Role.User:
                        return Role.UserAccess;
                }
            }

            return new string[] { };
        }

        public void Delete(string login)
        {
            accountDAL.Delete(login);
        }

        public string GetRole(string login)
        {
            var acc = Get(login);
            return  acc is null ? "" : acc.Role;
           
        }

        ///// <summary>
        ///// Если (админ или ведущий специалист) или Ответственный за задачу
        ///// </summary>
        //public bool IsAdminLeadResponsibleForTask(int taskId, string name)
        //{
        //    if (Role.AdminLeadList.Contains(GetRole(name)))
        //        return true;

        //    var task = Logic.Instance.Task.Get(taskId);

        //    if (task.Account.Login == name)
        //        return true;

        //    return false;
        //}

        //public AccountSettings GetAccountSettings(string login = null)
        //{
        //    if (login is null)
        //        login = CurrentUserLogin;

        //    var accSettings = new AccountSettings
        //    {
        //        NewStyle = false
        //    };

        //    if (login.Equals("DENDUKA"))
        //    {
        //        accSettings.NewStyle = true;
        //    }

        //    return accSettings;
        //}

        public void ClearCache()
        {
            accountDAL.ClearCache();
        }
    }
}