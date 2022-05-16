using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class SettingsLocalDAL : ISettingsDAL
    {
        public Settings Get()
        {
            return LocalStorage.Settings;
        }

        public OperationResult UpdateProjectIdForSystemAdmin(int projectId)
        {
            LocalStorage.Settings.ProjectIdForSystemAdmin = projectId;
            return new OperationResult(true);
        }

        public OperationResult UpdateSysAdmin(string sysAdminLogin)
        {
            LocalStorage.Settings.SystemAdminLogin = sysAdminLogin;
            return new OperationResult(true);
        }
    }
}
