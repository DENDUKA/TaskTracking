using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using TaskTrackingSystem.AbstractDAL.Access;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.DAL.DAL.Access
{
    public class ProjectTypeAccessDAL : IProjectTypeAccessDAL
    {
        private List<ProjectTypeAccess> _allProjectTypeAccesses = new List<ProjectTypeAccess>();
        private bool cacheIsFilled = false;
        private List<ProjectTypeAccess> AllProjectTypeAccesses
        {
            get
            {
                if (!cacheIsFilled)
                {
                    UpdateCache();
                }
                return _allProjectTypeAccesses;
            }
        }

        public List<ProjectTypeAccess> Get(string accountLogin)
        {
            return AllProjectTypeAccesses.FindAll(pta => pta.AccountLogin.Equals(accountLogin));
        }

        public List<ProjectTypeAccess> GetAll()
        {
            return AllProjectTypeAccesses;
        }

        public bool Delete(int id)
        {
            cacheIsFilled = false;

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM ProjectTypeAccess WHERE Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", id));

                con.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Edit(ProjectTypeAccess pta)
        {
            cacheIsFilled = false;

            var accessString = JsonConvert.SerializeObject(pta);

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE ProjectTypeAccess SET  ProjectTypeId = @ProjectTypeId, AccessString = @AccessString WHERE Id = @id;", con);
                command.Parameters.Add(new SqlParameter("@Id", pta.Id));
                command.Parameters.Add(new SqlParameter("@ProjectTypeId", pta.ProjectTypeId));
                command.Parameters.Add(new SqlParameter("@AccessString", accessString));

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

        public int Add(ProjectTypeAccess pta)
        {
            cacheIsFilled = false;

            var accessString = JsonConvert.SerializeObject(pta);

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO ProjectTypeAccess(AccountLogin, ProjectTypeId, AccessString) values(@AccountLogin, @ProjectTypeId, @AccessString) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@AccountLogin", pta.AccountLogin));
                command.Parameters.Add(new SqlParameter("@ProjectTypeId", pta.ProjectTypeId));
                command.Parameters.Add(new SqlParameter("@AccessString", accessString));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var id = (int)(decimal)reader["SCOPE_IDENTITY"];
                    pta.Id = id;

                    return id;
                }

                return 0;
            }
        }

        public void UpdateCache()
        {
            cacheIsFilled = true;
            AllProjectTypeAccesses.Clear();
            _allProjectTypeAccesses = GetAllFromDB();
        }

        private List<ProjectTypeAccess> GetAllFromDB()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM ProjectTypeAccess", con);
                con.Open();
                var reader = command.ExecuteReader();

                var ptas = new List<ProjectTypeAccess>();

                while (reader.Read())
                {
                    var pta = JsonConvert.DeserializeObject<ProjectTypeAccess>((string)reader["AccessString"]);
                    pta.Id = (int)reader["Id"];
                    pta.AccountLogin = (string)reader["AccountLogin"];
                    pta.ProjectTypeId = (int)reader["ProjectTypeId"];
                    ptas.Add(pta);
                }
                return ptas;
            }
        }

        public void ClearCache()
        {
            cacheIsFilled = false;
        }
    }
}
