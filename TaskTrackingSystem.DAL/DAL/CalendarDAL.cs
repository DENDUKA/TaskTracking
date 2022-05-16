using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class CalendarDAL : ICalendayDiaryEventDAL
    {
        public int Create(CalendarDiaryEvent diaryEvent)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO CalendarDiaryEvent(Name, DateStart, DateEnd) values(@Name, @DateStart, @DateEnd) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Name", diaryEvent.Name));
                command.Parameters.Add(new SqlParameter("@DateStart", diaryEvent.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", diaryEvent.DateEnd));

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
                var command = new SqlCommand("DELETE CalendarDiaryEvent WHERE id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));

                con.Open();

                return new OperationResult(command.ExecuteNonQuery() > 0, "");
            }
        }

        public CalendarDiaryEvent Get(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, Name FROM CalendarDiaryEvent WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadSQL(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<CalendarDiaryEvent> GetInRange(DateTime fromDate, DateTime toDate)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand(@"EXEC GetDiaryEventInRange @fromDate, @toDate ;", con);
                command.Parameters.Add(new SqlParameter("@fromDate", fromDate));
                command.Parameters.Add(new SqlParameter("@toDate", toDate));
                con.Open();
                var reader = command.ExecuteReader();

                var diaryEvents = new List<CalendarDiaryEvent>();

                while (reader.Read())
                {
                    diaryEvents.Add(ReadSQL(reader));
                }

                return diaryEvents;
            }
        }

        public OperationResult UpdateDates(CalendarDiaryEvent diaryEvent)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE CalendarDiaryEvent SET DateStart = @DateStart, DateEnd = @DateEnd WHERE Id = @Id;", con);
                command.Parameters.Add(new SqlParameter("@id", diaryEvent.Id));
                command.Parameters.Add(new SqlParameter("@DateStart", diaryEvent.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", diaryEvent.DateEnd));

                con.Open();
                try
                {
                    return new OperationResult(command.ExecuteNonQuery() > 0);
                }
                catch (Exception ex)
                {
                    return new OperationResult(false, ex.Message);
                }
            }
        }

        private CalendarDiaryEvent ReadSQL(SqlDataReader reader)
        {
            return new CalendarDiaryEvent((int)reader["Id"], (string)reader["Name"], (DateTime)reader["DateStart"], (DateTime)reader["DateEnd"]);
        }
    }
}
