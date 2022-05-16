using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class SettingsController : Controller
    {
        [Authorize]
        [ControllerAccess]
        public ActionResult Index()
        {
            var model = new SettingsModel();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ControllerAccess]
        public void UpdateSysAdmin(string sysAdminLogin)
        {
            Logic.Logic.Instance.Settings.UpdateSystemAdmin(sysAdminLogin);
        }

        [HttpPost]
        [Authorize]
        [ControllerAccess]
        public void UpdateProjectForSysAdmin(int projectId)
        {
            Logic.Logic.Instance.Settings.UpdateProjectIdForSystemAdmin(projectId);
        }
    }
}