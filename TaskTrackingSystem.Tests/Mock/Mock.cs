using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace TaskTrackingSystem.Tests.Mock
{
    public class FakeMock
    {
        public static HttpContext FakeHttpContext()
        {
            var uri = new Uri("http://localhost/");
            var httpRequest = new HttpRequest(string.Empty, uri.ToString(),
                                                uri.Query.TrimStart('?'));
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id",
                                            new SessionStateItemCollection(),
                                            new HttpStaticObjectsCollection(),
                                            10, true, HttpCookieMode.AutoDetect,
                                            SessionStateMode.InProc, false);

            SessionStateUtility.AddHttpSessionStateToContext(
                                                 httpContext, sessionContainer);

            return httpContext;
        }

        internal static void LoginUser(string login, string role)
        {
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(login), new string[] { role });
        }
    }
}
