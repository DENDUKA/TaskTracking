using System.Web;
using System.Web.Mvc;

namespace TaskTrackingSystem.Controllers
{
    public class TestController : Controller
    {
        [AccessAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }

    public class AccessAuthorizeAttribute : AuthorizeAttribute
    {
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var AccountLogin = httpContext.User.Identity.Name;

            var rd = httpContext.Request.RequestContext.RouteData;
            var currentAction = rd.GetRequiredString("action");
            var currentController = rd.GetRequiredString("controller");
           



            return true;
        }
    }
}