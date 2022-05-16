using System.Collections.Generic;
using System.Web;
using TaskTrackingSystem.AbstractDAL.Access;
using TaskTrackingSystem.DAL.DAL.Access;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.Logic.Access
{
    public class AccountAccessBLL
    {
        #region singleton
        public static AccountAccessBLL Instance { get; private set; }

        private readonly IProjectTypeAccessDAL projectTypeAccessDAL;
        private readonly IAccountAccessDAL userAccessDAL;
        private readonly IProjectAccessDAL projectAccessDAL;

        private AccountAccessBLL()
        {
            userAccessDAL = new AccountAccessDAL();
            projectTypeAccessDAL = new ProjectTypeAccessDAL();
            projectAccessDAL = new ProjectAccessDAL();
        }

        static AccountAccessBLL()
        {
            Instance = new AccountAccessBLL();
        }

        #endregion

        internal void CreateMissingAccounts()
        {
            foreach (var acc in Logic.Instance.Account.GetAll())
            {
                if (userAccessDAL.Get(acc.Login) is null)
                {
                    userAccessDAL.Add(new AccountAccess(acc.Login, new List<Shared.Entities.Access.ProjectTypeAccess>()));
                }
            }
        }

        internal void ClearCache()
        {
            userAccessDAL.ClearCache();
            projectTypeAccessDAL.ClearCache();
            projectAccessDAL.ClearCache();
        }

        public AccountAccess GetAccessForAccount()
        {
            return GetAccessForAccount(HttpContext.Current.User.Identity.Name);
        }

        public AccountAccess GetAccessForAccount(string accountLogin)
        {
            var userAccess = userAccessDAL.Get(accountLogin);

            if (userAccess != null)
            { 
                userAccess.ProjectTypeAccesses = projectTypeAccessDAL.Get(accountLogin);
                userAccess.ProjectAccesses = projectAccessDAL.Get(accountLogin);
            }

            return userAccess;
        }

        //Возвращает права доступа для аккаунта и ко всем связанным с ним проектам
        public List<AccountAccess> GetFullAccessForAllAccounts()
        {
            var accesses = new List<AccountAccess>();

            foreach (var account in Logic.Instance.Account.GetAll())
            {
                var access = userAccessDAL.Get(account.Login);
                if (userAccessDAL.Get(account.Login) != null)
                {
                    access.ProjectTypeAccesses = projectTypeAccessDAL.Get(account.Login);
                    access.ProjectAccesses = projectAccessDAL.Get(account.Login);

                    accesses.Add(access);
                }
            }

            return accesses;
        }

        #region AccountAccess

        public bool IsAdmin(string login = null)
        {
            if (login is null)
            {
                login = HttpContext.Current.User.Identity.Name;
            }

            return Logic.Instance.Account.GetRole(login).Equals(Shared.Entities.Role.Admin);
        }

        public bool IsAdminOrBuh(string login = null)
        {
            if (login is null)
            {
                login = HttpContext.Current.User.Identity.Name;
            }

            var role = Logic.Instance.Account.GetRole(login);

            return role.Equals(Shared.Entities.Role.Admin) || role.Equals(Shared.Entities.Role.Buh);
        }

        public bool IsCurrentUser(string login)
        {
            if (login is null)
            {
                return false;
            }

            return login.Equals(HttpContext.Current.User.Identity.Name);
        }

        public bool AccountAccessEdit(AccountAccess accountAccess)
        {
            return userAccessDAL.Edit(accountAccess);
        }


        #endregion

        #region ProjectAccess

        public bool ProjectDelete(int id)
        {
            return projectAccessDAL.Delete(id);
        }
        public bool ProjectEdit(Shared.Entities.Access.ProjectAccess projectAccess)
        {
            return projectAccessDAL.Edit(projectAccess);
        }
        public int ProjectAdd(Shared.Entities.Access.ProjectAccess projectAccess)
        {
            return projectAccessDAL.Add(projectAccess);
        }

        public List<Shared.Entities.Access.ProjectAccess> GetAccessForProject(int projectId)
        {
            var projectsAccess = projectAccessDAL.GetAll().FindAll(pa => pa.ProjectId == projectId);
            foreach (var pa in projectsAccess)
            {
                pa.Account = Logic.Instance.Account.Get(pa.AccountLogin);
            }
            return projectsAccess;
        }

        #endregion

        #region ProjectTypeAccess

        public bool ProjectTypeDelete(int id)
        {
            return projectTypeAccessDAL.Delete(id);
        }
        public bool ProjectTypeEdit(Shared.Entities.Access.ProjectTypeAccess projectTypeAccess)
        {
            return projectTypeAccessDAL.Edit(projectTypeAccess);
        }
        public int ProjectTypeAdd(Shared.Entities.Access.ProjectTypeAccess projectTypeAccess)
        {
            return projectTypeAccessDAL.Add(projectTypeAccess);
        }

        #endregion
    }
}
