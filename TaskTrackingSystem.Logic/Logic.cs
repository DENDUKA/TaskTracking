using TaskTrackingSystem.Logic.Access;
using TaskTrackingSystem.Logic.ActionLogs;
using TaskTrackingSystem.Logic.Network;
using TaskTrackingSystem.Logic.PCOnline;

namespace TaskTrackingSystem.Logic
{
    public class Logic
    {
        #region singleton
        public static Logic Instance { get; private set; }

        static Logic()
        {
            Instance = new Logic();
        }

        private Logic()
        {           
        }
        #endregion

        public TaskBLL Task => TaskBLL.Instance;
        public ProjectBLL Project => ProjectBLL.Instance;
        public AccountBLL Account => AccountBLL.Instance;
        public ProjectTypeBLL ProjectType => ProjectTypeBLL.Instance;
        public StatusBLL Status => StatusBLL.Instance;
        public PCOnlineBLL PCOnline => PCOnlineBLL.Instance;
        public AccessBLL Access => AccessBLL.Instance;
        public AccountAccessBLL AccountAccess => AccountAccessBLL.Instance;
        public ActionLogsBLL ActionLogs => ActionLogsBLL.Instance;
        public CacheBLL Cache => CacheBLL.Instance;
        public CompanyBLL Company => CompanyBLL.Instance;
        public CommentBLL Comment => CommentBLL.Instance;
        public EmployeeBLL Employee => EmployeeBLL.Instance;
        public SettingsBLL Settings => SettingsBLL.Instance;
        public CalendarBLL CalendarDiaryEvent => CalendarBLL.Instance;
        public TimeTrackBLL TimeTrack => TimeTrackBLL.Instance;
        public Log.LogEvent LogEvent => Log.LogEvent.Instance;
        public PayListBLL PayList => PayListBLL.Instance;
        public PCNetworkInfoBLL PCNetworkInfo => PCNetworkInfoBLL.Instance;
    }
}