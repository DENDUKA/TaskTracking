using System.Web.Mvc;

namespace TaskTrackingSystem.Filters
{
    public class AuthorizeAttributeExt : AuthorizeAttribute
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}