using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class SettingsModel
    {
        public SettingsModel()
        {
            Settings = Logic.Logic.Instance.Settings.Get();
            AccountLogin = Settings.SystemAdminLogin;
            ProjectIdForSysAdmin = Settings.ProjectIdForSystemAdmin;
        }

        [DisplayName("Системный администратор")]
        public Account Account { get; set; }
        public string AccountLogin { get; set; }
        [DisplayName("Проект для задач сис админа")]
        public int ProjectIdForSysAdmin { get; set; }
        private Settings Settings { get; set; }

        public IEnumerable<SelectListItem> DropDownListAccounts => ModelHelper.GetDropDownListAccounts(Settings.SystemAdminLogin);
        public IEnumerable<SelectListItem> DropDownListSelectProjectForSysAdmin => ModelHelper.GetDropDownListProjectForProjectType(ProjectType.OtherProjects);

    }

}