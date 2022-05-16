using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ISettingsDAL
    {
        Settings Get();
        OperationResult UpdateSysAdmin(string sysAdminLogin);
        OperationResult UpdateProjectIdForSystemAdmin(int projectId);
    }
}
