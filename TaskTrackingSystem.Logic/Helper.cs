using System.IO;

namespace TaskTrackingSystem.Logic
{
    public static class Helper
    {
        public const string FtpRoot = @"\\192.168.1.168\Work";

        static public string GetRoot(string path)
        {
            var root = Path.GetPathRoot(path);
            return root.Remove(root.LastIndexOf('\\'), root.Length - root.LastIndexOf('\\'));
        }

        static public string ChangeRoot(string path, string root = FtpRoot)
        {
            return root + path.Replace(GetRoot(path), "");
        }
    }
}
