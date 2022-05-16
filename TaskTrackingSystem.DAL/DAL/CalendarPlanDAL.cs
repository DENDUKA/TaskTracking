using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class CalendarPlanDAL : ICalendarPlanDAL
    {
        public CalendarPlan Get(int projectId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM CalendarPlan WHERE ProjectId = @ProjectId ORDER BY StageNum", con);
                command.Parameters.Add(new SqlParameter("@ProjectId", projectId));
                con.Open();

                var reader = command.ExecuteReader();

                return ReadCalendarPlan(reader);
            }
        }

        public int CreateCalendarPlanItem(int projectId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO CalendarPlan(ProjectId, StageNum, WorkType, Deadlines) " +
                    "values(@ProjectId, @StageNum, @WorkType, @Deadlines) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@ProjectId", projectId));
                command.Parameters.Add(new SqlParameter("@StageNum", "0"));
                command.Parameters.Add(new SqlParameter("@WorkType", ""));
                command.Parameters.Add(new SqlParameter("@Deadlines", ""));

                con.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["SCOPE_IDENTITY"];
                }

                return 0;
            }
        }

        public OperationResult UpdateCalendarPlanItem(CalendarPlanItem calendarPlanItem)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE CalendarPlan SET StageNum = @StageNum, WorkType = @WorkType, Deadlines = @Deadlines  WHERE Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", calendarPlanItem.Id));
                command.Parameters.Add(new SqlParameter("@StageNum", calendarPlanItem.StageNum));
                command.Parameters.Add(new SqlParameter("@WorkType", calendarPlanItem.WorkType));
                command.Parameters.Add(new SqlParameter("@Deadlines", calendarPlanItem.Deadlines));

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

        public OperationResult DeleteCalendarPlanItem(int calendarPlanItemId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM CalendarPlan WHERE Id = @CalendarPlanItemId", con);
                command.Parameters.Add(new SqlParameter("@CalendarPlanItemId", calendarPlanItemId));

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

        private CalendarPlan ReadCalendarPlan(SqlDataReader reader)
        {
            var items = new List<CalendarPlanItem>();
            var projectId = 0;
            var isContains = false;

            while (reader.Read())
            {
                if (!isContains)
                {
                    projectId = (int)reader["projectId"];
                }

                items.Add(ReadCalendarPlanItem(reader));

                isContains = true;
            }

            foreach (var x in items)
            {
                Console.WriteLine(x.Id);
            }

            return isContains ? new CalendarPlan(projectId, items) : null;
        }

        private CalendarPlanItem ReadCalendarPlanItem(SqlDataReader reader)
        {
            return new CalendarPlanItem(
                         (int)reader["Id"],
                         (int)reader["StageNum"],
                         (string)reader["WorkType"],
                         (string)reader["Deadlines"]
                         );
        }
    }
}