using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class CommentDAL : ICommentDAL
    {
        public int AddForTask(Comment comment)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Comment(TaskId, Login, Time, Text) values(@TaskId, @Login, @Time, @Text) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@TaskId", comment.TaskId));
                command.Parameters.Add(new SqlParameter("@Login", comment.Login));
                command.Parameters.Add(new SqlParameter("@Time", comment.Time));
                command.Parameters.Add(new SqlParameter("@Text", comment.Text));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["SCOPE_IDENTITY"];
                }

                return 0;
            }
        }

        public OperationResult Delete(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE Comment WHERE id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                return new OperationResult(command.ExecuteNonQuery() > 0, "");
            }
        }

        public Comment Get(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, TaskId, Login, Time, Text FROM Comment WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return ReadComment(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Comment> GetForTask(int taskId)
        {
            var comments = new List<Comment>();

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, TaskId, Login, Time, Text FROM Comment WHERE TaskId = @TaskId", con);
                command.Parameters.Add(new SqlParameter("@TaskId", taskId));
                con.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comments.Add(ReadComment(reader));
                }
            }
            return comments;
        }

        private Comment ReadComment(SqlDataReader reader)
        {
            return new Comment((int)reader["Id"], (int)reader["TaskId"], (string)reader["Login"], (DateTime)reader["Time"], (string)reader["Text"]);
        }
    }
}