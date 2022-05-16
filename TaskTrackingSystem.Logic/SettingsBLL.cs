using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class SettingsBLL
    {
        #region singleton
        public static SettingsBLL Instance { get; private set; }

        private readonly ISettingsDAL settingsDAL;

        private SettingsBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    settingsDAL = new SettingsDAL();
                    break;
                case "Local":
                    settingsDAL = new SettingsLocalDAL();
                    break;
                case "EFDB":
                    settingsDAL = new EFDAL.SettingsDAL();
                    break;
            }
        }

        static SettingsBLL()
        {
            Instance = new SettingsBLL();
        }
        #endregion

        public Settings Get()
        {
            return settingsDAL.Get();
        }

        public OperationResult UpdateSystemAdmin(string sysAdminLogin)
        {
            return settingsDAL.UpdateSysAdmin(sysAdminLogin);
        }

        public OperationResult UpdateProjectIdForSystemAdmin(int projectId)
        {
            return settingsDAL.UpdateProjectIdForSystemAdmin(projectId);
        }
    }
}
