using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class ProjectDAL : IProjectDAL
    {
        public Project Get(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetProjectById @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var project = ReadProject(reader, true);
                    project.StoryPoints = GetProjectStoryPoint(id);

                    return project;
                }
                else
                {
                    return null;
                }
            }
        }

        private int GetProjectStoryPoint(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetProjectStoryPoints @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader["ProjectStoryPoints"] == DBNull.Value ? 0 : (int)reader["ProjectStoryPoints"];
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<Project> GetAllForProjectType(int projTypeId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetProjectsByProjectTypeId @projTypeId", con);
                command.Parameters.Add(new SqlParameter("@projTypeId", projTypeId));
                con.Open();
                var reader = command.ExecuteReader();

                var projects = new List<Project>();

                while (reader.Read())
                {
                    projects.Add(ReadProject(reader, true));
                }

                return projects;
            }
        }

        public List<Project> GetAllForCompany(int companyId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetProjectsForCompany @CompanyId", con);
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                con.Open();
                var reader = command.ExecuteReader();

                var projects = new List<Project>();

                while (reader.Read())
                {
                    projects.Add(ReadProject(reader, true));
                }

                return projects;
            }
        }

        public int Create(Project project)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Project(Name, Responsible, DateStart, DateEnd, DateEndFact, ProjectType, Status, ContractNumber, Cost, Paid, CompanyId) values(@Name, @accountLogin, @DateStart, @DateEnd, @DateEndFact, @projectTypeId, @status, @contractNumber, @cost, @paid, @companyId) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Name", project.Name));
                command.Parameters.Add(new SqlParameter("@accountLogin", project.Account.Login));
                command.Parameters.Add(new SqlParameter("@DateStart", project.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", project.DateEnd));
                command.Parameters.Add(new SqlParameter("@DateEndFact", project.DateEndFact));
                command.Parameters.Add(new SqlParameter("@projectTypeId", project.ProjectType.Id));
                command.Parameters.Add(new SqlParameter("@status", project.Status.Id));
                command.Parameters.Add(new SqlParameter("@contractNumber", project.ContractNumber ?? ""));
                command.Parameters.Add(new SqlParameter("@cost", project.Cost));
                command.Parameters.Add(new SqlParameter("@paid", project.Paid));
                command.Parameters.Add(new SqlParameter("@companyId", project.CompanyId));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["SCOPE_IDENTITY"];
                }

                return 0;
            }
        }

        public List<Project> GetAllForAccount(string login)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("EXEC GetProjectsForAccount @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", login));
                con.Open();
                var reader = command.ExecuteReader();

                var projects = new List<Project>();

                while (reader.Read())
                {
                    projects.Add(ReadProject(reader, false));
                }

                return projects;
            }
        }

        public OperationResult UpdateStatus(int id, int statusId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Project SET Status = @statusId WHERE Id = @id;", con);
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

        public OperationResult Delete(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Project WHERE id=@id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                try
                {
                    if (command.ExecuteNonQuery() > 0)
                        return new OperationResult(true, null);
                    else
                        return new OperationResult(false, "ID не найден");
                }
                catch (Exception e)
                {
                    return new OperationResult(false, e.Message);
                }
            }
        }

        public bool Update(Project project)
        {
            var updateCost = project.Cost != 0;

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand($"UPDATE Project SET Name = @name, DateStart = @dateStart, DateEnd = @DateEnd, DateEndFact = @DateEndFact, " +
                    $"Status = @statusId, CompanyId = @companyId, Paid = @paid, " +
                    $"ContractNumber = @contractNumber { (updateCost ? ",Cost = @cost" : "") }   WHERE Id = @id;", con);

                command.Parameters.Add(new SqlParameter("@id", project.Id));
                command.Parameters.Add(new SqlParameter("@name", project.Name));
                command.Parameters.Add(new SqlParameter("@dateStart", project.DateStart));
                command.Parameters.Add(new SqlParameter("@DateEnd", project.DateEnd));
                command.Parameters.Add(new SqlParameter("@DateEndFact", project.DateEndFact));                
                command.Parameters.Add(new SqlParameter("@statusId", project.Status.Id));
                command.Parameters.Add(new SqlParameter("@contractNumber", project.ContractNumber ?? ""));
                command.Parameters.Add(new SqlParameter("@companyId", project.CompanyId));
                command.Parameters.Add(new SqlParameter("@paid", project.Paid));
                if (updateCost)
                    command.Parameters.Add(new SqlParameter("@cost", project.Cost));

                con.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        private Project ReadProject(SqlDataReader reader, bool readAccount)
        {
            var CompanyId = 0;
            if (reader["CompanyId"] != DBNull.Value)
            {
                CompanyId = (int)reader["CompanyId"];
            }

            return new Project(
                        (int)reader["Id"],
                        (string)reader["Name"],
                        readAccount ? new Account((string)reader["AccName"], (string)reader["Responsible"]) : null,
                        new ProjectType((int)reader["ProjectType"], (string)reader["PtName"]),
                        new Status((int)reader["Status"], (string)reader["StatusName"]),
                        "",
                        (DateTime)reader["DateStart"],
                        (DateTime)reader["DateEnd"],
                        (DateTime)reader["DateEndFact"],
                        Shared.NotNullRead(reader["ContractNumber"]),
                        reader["Cost"] == DBNull.Value ? 0 : (decimal)reader["Cost"],
                        (decimal)reader["Paid"],
                        CompanyId,
                        (bool)reader["PremiumPaid"]
                        );
        }

        public List<ProjectPathToFile> GetFilePaths(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM ProjectPathToFile WHERE ProjectId = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", id));
                con.Open();
                var reader = command.ExecuteReader();

                var filePaths = new List<ProjectPathToFile>();

                while (reader.Read())
                {
                    filePaths.Add(ReadPathToFile(reader, false));
                }

                return filePaths;
            }
        }

        private ProjectPathToFile ReadPathToFile(SqlDataReader reader, bool v)
        {
            return new ProjectPathToFile(
                         (int)reader["Id"],
                         (int)reader["projectId"],
                         (string)reader["Description"],
                         (string)reader["Path"]
                         );
        }

        public int CreateFilePath(int projectId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO ProjectPathToFile(ProjectId, Description, Path) values(@projectId, '', '') SELECT SCOPE_IDENTITY() AS [Id];", con);
                command.Parameters.Add(new SqlParameter("@projectId", projectId));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["Id"];
                }

                return 0;
            }
        }

        public OperationResult DeleteFilePath(int filePathId)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM ProjectPathToFile WHERE id=@filePathId", con);
                command.Parameters.Add(new SqlParameter("@filePathId", filePathId));
                con.Open();
                try
                {
                    if (command.ExecuteNonQuery() > 0)
                        return new OperationResult(true, null);
                    else
                        return new OperationResult(false, "ID не найден");
                }
                catch (Exception e)
                {
                    return new OperationResult(false, e.Message);
                }
            }
        }

        public OperationResult UpdateFilePath(ProjectPathToFile filePath)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand($"UPDATE ProjectPathToFile SET Description = @description, Path = @path WHERE Id = @filePathId;", con);

                command.Parameters.Add(new SqlParameter("@filePathId", filePath.Id));
                command.Parameters.Add(new SqlParameter("@description", filePath.Description));
                command.Parameters.Add(new SqlParameter("@path", filePath.Path));

                con.Open();

                try
                {
                    if (command.ExecuteNonQuery() > 0)
                        return new OperationResult(true, null);
                    else
                        return new OperationResult(false, "ID не найден");
                }
                catch (Exception e)
                {
                    return new OperationResult(false, e.Message);
                }
            }
        }

        public void ChangeProjectType(int projectId, int projectTypeId)
        {
            throw new NotImplementedException();
        }
    }
}