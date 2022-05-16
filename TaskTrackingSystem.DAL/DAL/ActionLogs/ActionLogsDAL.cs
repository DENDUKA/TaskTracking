using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL.Logs;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.ActionLogs
{
    public class ActionLogsDAL : IActionLogsDAL
    {
        public void Add(int typeId, string userLogin, string fieldName, string oldValue, string newValue, string type)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO ActionLogs(TypeId, Time, FieldName, UserLogin, ChangedValue, NewValue, Type) " +
                    "values(@TypeId, @Time, @FieldName, @UserLogin, @ChangedValue, @NewValue, @Type)", con);

                command.Parameters.Add(new SqlParameter("@TypeId", typeId));
                command.Parameters.Add(new SqlParameter("@Time", DateTime.Now));
                command.Parameters.Add(new SqlParameter("@FieldName", fieldName));
                command.Parameters.Add(new SqlParameter("@UserLogin", userLogin));
                command.Parameters.Add(new SqlParameter("@ChangedValue", oldValue is null ? "" : oldValue));
                command.Parameters.Add(new SqlParameter("@NewValue", newValue is null ? "" : newValue));
                command.Parameters.Add(new SqlParameter("@Type", type));

                con.Open();
                var reader = command.ExecuteReader();
            }
        }

        public List<ActionLog> Read(int typeId, string type)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var logs = new List<ActionLog>();

                var command = new SqlCommand("SELECT * FROM ActionLogs WHERE TypeId = @TypeId AND Type = @Type ORDER BY Time DESC", con);
                command.Parameters.Add(new SqlParameter("@TypeId", typeId));
                command.Parameters.Add(new SqlParameter("@Type", type));

                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(new ActionLog()
                    {
                        TypeId = (int)reader["TypeId"],
                        UserLogin = (string)reader["UserLogin"],
                        Time = (DateTime)reader["Time"],
                        FieldName = (string)reader["FieldName"],
                        NewValue = (string)reader["NewValue"],
                        ChangedValue = (string)reader["ChangedValue"],
                        Type = (string)reader["Type"],
                    });
                }

                return logs;
            }
        }
    }
}
