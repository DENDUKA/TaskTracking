using System.Text.RegularExpressions;

namespace TaskTrackingSystem.Shared.Static
{
    public static class Regexes
    {
        public static readonly Regex EmailRegex = new Regex(@"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}");
    }
}
