using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class TimeTrackDAL : ITimeTrackDAL
    {       
        public List<TimeTrack> GetAll()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM TimeTrack as tt LEFT JOIN Account AS a ON a.Login = tt.Login ORDER BY tt.DateStart DESC", con);
                con.Open();
                var reader = command.ExecuteReader();

                var timeTracks = new List<TimeTrack>();

                while (reader.Read())
                {
                    timeTracks.Add(ReadSQL(reader));
                }

                return timeTracks;
            }
        }

        public List<TimeTrack> GetAllForAccount(string login)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM TimeTrack as tt LEFT JOIN Account AS a ON a.Login = tt.Login WHERE tt.Login = @login ORDER BY tt.DateStart DESC", con);
                command.Parameters.Add(new SqlParameter("@login", login));
                con.Open();
                var reader = command.ExecuteReader();

                var timeTracks = new List<TimeTrack>();

                while (reader.Read())
                {
                    timeTracks.Add(ReadSQL(reader));
                }

                return timeTracks;
            }
        }

        public bool Delete(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM TimeTrack WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));

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

        public int Create(TimeTrack timeTrack)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO TimeTrack(Login, Type, DateStart, DateEnd, Description) values(@Login, @Type, @DateStart, @DateEnd, @Description) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Login", timeTrack.Account.Login));
                command.Parameters.Add(new SqlParameter("@Type", timeTrack.Type));
                command.Parameters.Add(new SqlParameter("@DateStart", timeTrack.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", timeTrack.DateEnd));
                command.Parameters.Add(new SqlParameter("@Description", timeTrack.Description));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["SCOPE_IDENTITY"];
                }

                return 0;
            }
        }

        public bool Update(TimeTrack timeTrack)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE TimeTrack SET Login = @Login, Type = @Type, DateStart = @DateStart, DateEnd = @DateEnd, Description = @Description WHERE Id = @id;", con);
                command.Parameters.Add(new SqlParameter("@Login", timeTrack.Account.Login));
                command.Parameters.Add(new SqlParameter("@Type", timeTrack.Type));
                command.Parameters.Add(new SqlParameter("@DateStart", timeTrack.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", timeTrack.DateEnd));

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

        private TimeTrack ReadSQL(SqlDataReader reader)
        {
            return new TimeTrack((int)reader["Id"],
                new Account(Shared.NotNullRead(reader["Name"]), Shared.NotNullRead(reader["Login"])),
                (string)reader["Type"], (DateTime)reader["DateStart"], (DateTime)reader["DateEnd"], (string)reader["Description"]);
        }
    }
}