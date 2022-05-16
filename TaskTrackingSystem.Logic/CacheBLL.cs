namespace TaskTrackingSystem.Logic
{
    public class CacheBLL
    {
        #region singleton
        public static CacheBLL Instance { get; private set; }

        private CacheBLL()
        {
        }

        static CacheBLL()
        {
            Instance = new CacheBLL();
        }
        #endregion

        public void Clear()
        {
            Logic.Instance.Company.ClearCache();
            Logic.Instance.Status.ClearCache();
            Logic.Instance.Account.ClearCache();
            Logic.Instance.AccountAccess.ClearCache();
            Logic.Instance.AccountAccess.CreateMissingAccounts();
        }
    }
}