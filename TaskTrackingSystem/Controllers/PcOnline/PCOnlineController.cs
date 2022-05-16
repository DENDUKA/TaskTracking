using System.Web.Mvc;
using TaskTrackingSystem.Models.PCOnline;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers.PcOnline
{
    public class PCOnlineController : Controller
    {
        [Authorize(Roles = Role.Admin)]
        public ActionResult Index()
        {
            var model = new PCOnlineModel
            {
                CorrentStatuses = Logic.Logic.Instance.PCOnline.GetLocation()
            };

            return View(model);
        }
    }
}