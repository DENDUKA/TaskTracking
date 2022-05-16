using System;
using System.Configuration;

namespace TaskTrackingSystem.DAL
{
    public static class Shared
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["TaskTrackingSystem"].ConnectionString;
        public static string DALType = "EFDB";

        internal static string NotNullRead(object obj)
        {
            return obj == DBNull.Value ? "" : (string)obj;
        }
        internal static string NullRead(object obj)
        {
            return obj == DBNull.Value ? null : (string)obj;
        }
    }
}
