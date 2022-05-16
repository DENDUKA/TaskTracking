using System;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class SettingsDAL : ISettingsDAL
    {
        public Settings Get()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Settings", con);

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Settings((string)reader["SystemAdmin"], (int)reader["ProjectIdForSystemAdmin"]);
                }

                return new Settings("", 0);
            }
        }

        public OperationResult UpdateSysAdmin(string sysAdminlogin)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Settings SET SystemAdmin = @SystemAdmin", con);
                command.Parameters.Add(new SqlParameter("@SystemAdmin", sysAdminlogin));

                con.Open();
                try
                {
                    return new OperationResult(command.ExecuteNonQuery() > 0, "");
                }
                catch (Exception e)
                {
                    return new OperationResult(false, e.Message);
                }
            }
        }

        public OperationResult UpdateProjectIdForSystemAdmin(int projectId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Settings SET ProjectIdForSystemAdmin = @projectId", con);
                command.Parameters.Add(new SqlParameter("@projectId", projectId));

                con.Open();
                try
                {
                    return new OperationResult(command.ExecuteNonQuery() > 0, "");
                }
                catch (Exception e)
                {
                    return new OperationResult(false, e.Message);
                }
            }
        }
    }
}
