using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class TaskDAL : ITaskDAL
    {
        public Task Get(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetTaskById @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return ReadTask(reader, true, true);
                }
                else
                {
                    return null;
                }
            }
        }
        public List<Task> GetAllForProject(int projectId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetTasksByProjectId @ProjectId", con);
                command.Parameters.Add(new SqlParameter("@ProjectId", projectId));
                con.Open();
                var reader = command.ExecuteReader();

                var tasks = new List<Task>();
                while (reader.Read())
                {
                    tasks.Add(ReadTask(reader, false, true));
                }

                return tasks;
            }
        }

        public int Add(Task task)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
               
                var command = new SqlCommand("INSERT INTO Task(Name, Project, DateStart, DateEnd, Status, Account, Description, StoryPoints, Reporter) values(@Name, @ProjectId, @DateStart, @DateEnd, @StatusId, @Account, @Description, @StoryPoints, @Reporter) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Name", task.Name));
                command.Parameters.Add(new SqlParameter("@ProjectId", task.ProjectId));
                command.Parameters.Add(new SqlParameter("@DateStart", task.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", task.DateEnd));
                command.Parameters.Add(new SqlParameter("@StatusId", task.Status.Id));
                command.Parameters.Add(new SqlParameter("@Account", task.Account.Login));
                command.Parameters.Add(new SqlParameter("@Description", task.Description));
                command.Parameters.Add(new SqlParameter("@StoryPoints", task.StoryPoints));
                if (task.Reporter != null && !string.IsNullOrEmpty(task.Reporter.Login))
                {
                    command.Parameters.Add(new SqlParameter("@Reporter", task.Reporter.Login));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@Reporter", DBNull.Value));
                }

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["SCOPE_IDENTITY"];
                }

                return 0;
            }
        }

        public OperationResult UpdateStatus(int id, int statusId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Task SET Status = @statusId WHERE Id = @id;", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@statusId", statusId));

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

        public bool UpdateMainFiels(Task task)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Task SET Name = @name, DateStart = @dateStart, DateEnd = @DateEnd, Account = @account, Description = @description, Status = @statusId, StoryPoints = @StoryPoints WHERE Id = @id;", con);
                command.Parameters.Add(new SqlParameter("@id", task.Id));
                command.Parameters.Add(new SqlParameter("@name", task.Name));
                command.Parameters.Add(new SqlParameter("@dateStart", task.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", task.DateEnd));
                command.Parameters.Add(new SqlParameter("@account", task.Account.Login));
                command.Parameters.Add(new SqlParameter("@description", task.Description));
                command.Parameters.Add(new SqlParameter("@statusId", task.Status.Id));
                command.Parameters.Add(new SqlParameter("@StoryPoints", task.StoryPoints));

                con.Open();
                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public List<Task> GetAllForAccount(string login)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetTasksForAccount @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", login));
                con.Open();
                var reader = command.ExecuteReader();

                var tasks = new List<Task>();

                while (reader.Read())
                {
                    tasks.Add(ReadTask(reader, false, false));
                }

                return tasks;
            }
        }

        public OperationResult Delete(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE Task WHERE id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));

                con.Open();

                return new OperationResult(command.ExecuteNonQuery() > 0, "");
            }
        }

        private Task ReadTask(SqlDataReader reader, bool readDescription, bool readAccount)
        {
            return new Task(
                       (int)reader["Id"],
                       (string)reader["TaskName"],
                       (DateTime)reader["DateStart"],
                       (DateTime)reader["DateEnd"],
                       new Status((int)reader["StatusId"], (string)reader["StatusName"]),
                       (int)reader["ProjectId"],
                       (string)reader["ProjectName"],
                       readAccount ? new Account(Shared.NotNullRead(reader["AccountName"]), Shared.NotNullRead(reader["AccountLogin"])) : null,
                       readDescription ? Shared.NotNullRead(reader["Description"]) : "",
                       new Account(Shared.NotNullRead(reader["ReporterName"]), Shared.NotNullRead(reader["ReporterLogin"])),
                       (int)(reader["StoryPoints"] == DBNull.Value ? 0 : reader["StoryPoints"]));
        }

        public List<Task> GetAll(DateTime start, DateTime end, int[] statusId)
        {
            throw new NotImplementedException();
        }
    }
}