using System.Collections.Generic;

namespace TaskTrackingSystem.AccessAuthorizeAttribute
{
    public static class AccessAuthorizeHelper
    {
        //Dictionary< "Controller" , "Dictionary< 'Action', 'AccessField'>">
        public static readonly Dictionary<string, Dictionary<string, string>> AccessToActions = new Dictionary<string, Dictionary<string, string>>
        {
            { "Account",
                new Dictionary<string, string>()
                {
                    {"Create", "AccountCreate"},
                }
            },



        };

    }
}