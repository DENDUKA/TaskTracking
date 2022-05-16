using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class ProjectTypeDAL : IProjectTypeDAL
    {

/*
* Геолого-разведочные работы
* Сейсморазведка
* Подсчет запасов
* Разработка
* Другие проекты
*/
        public List<ProjectType> GetAll()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, Name FROM ProjectType", con);
                con.Open();
                var reader = command.ExecuteReader();

                var projects = new List<ProjectType>();

                while (reader.Read())
                {
                    projects.Add(new ProjectType((int)reader["Id"], (string)reader["Name"]));
                }

                return projects;
            }
        }

        public ProjectType Get(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, Name FROM ProjectType WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new ProjectType((int)reader["Id"], (string)reader["Name"]);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}