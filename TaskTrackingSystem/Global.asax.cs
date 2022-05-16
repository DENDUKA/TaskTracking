using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.UI.Web;


namespace TaskTrackingSystem
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer<AccountContext>(null);
        }
    }
}
