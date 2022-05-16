using System;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.EFDAL
{
    public class SettingsDAL : ISettingsDAL
    {
        public Settings Get()
        {
            using (var db = new SettingContext())
            {
                return db.Settings
                     .FirstOrDefault();
            }
        }

        public OperationResult UpdateSysAdmin(string sysAdminlogin)
        {
            using (var db = new SettingContext())
            {
                var setting = db.Settings.First();

                setting.SystemAdminLogin = sysAdminlogin;

                db.SaveChanges();
            }

            return new OperationResult(true);
        }

        public OperationResult UpdateProjectIdForSystemAdmin(int projectId)
        {
            using (var db = new SettingContext())
            {
                var setting = db.Settings.First();

                setting.ProjectIdForSystemAdmin = projectId;

                db.SaveChanges();
            }

            return new OperationResult(true);
        }
    }
}