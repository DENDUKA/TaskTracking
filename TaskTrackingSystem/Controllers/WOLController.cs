using System.Web.Mvc;

namespace TaskTrackingSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class WOLController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}