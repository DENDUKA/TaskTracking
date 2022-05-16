using System.Web;

namespace TaskTrackingSystem.Filters
{
    public class AuthorizationFilter : AuthorizeAttributeExt
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var login = HttpContext.Current.User.Identity.Name;

            var account = Logic.Logic.Instance.Account.Get(login);
            return account.Role.Equals(Shared.Entities.Role.Admin);
        }
    }
}