using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using TaskTrackingSystem.AbstractDAL.Access;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.EFDAL.Access
{
    public  class ProjectAccessDAL : IProjectAccessDAL
    {
        private List<ProjectAccess> _allProjectAccesses = new List<ProjectAccess>();
        private bool cacheIsFilled = false;
        private List<ProjectAccess> AllProjectAccesses
        {
            get
            {
                if (!cacheIsFilled)
                {
                    UpdateCache();
                }
                return _allProjectAccesses;
            }
        }

        public List<ProjectAccess> Get(string accountLogin)
        {
            return AllProjectAccesses.FindAll(pta => pta.AccountLogin.Equals(accountLogin));
        }

        public List<ProjectAccess> GetAll()
        {
            return AllProjectAccesses;
        }

        public bool Delete(int id)
        {
            cacheIsFilled = false;

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM ProjectAccess WHERE Id = @Id", con);
                command.Parameters.Add(new SqlParameter("@Id", id));

                con.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Edit(ProjectAccess pa)
        {
            cacheIsFilled = false;

            var accessString = JsonConvert.SerializeObject(pa);

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE ProjectAccess SET AccountLogin = @AccountLogin,  AccessString = @AccessString WHERE Id = @id;", con);
                command.Parameters.Add(new SqlParameter("@Id", pa.Id));
                command.Parameters.Add(new SqlParameter("@AccountLogin", pa.AccountLogin));
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

        public int Add(ProjectAccess pta)
        {
            cacheIsFilled = false;

            var accessString = JsonConvert.SerializeObject(pta);

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO ProjectAccess(AccountLogin, ProjectId, AccessString) values(@AccountLogin, @ProjectId, @AccessString) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@AccountLogin", pta.AccountLogin));
                command.Parameters.Add(new SqlParameter("@ProjectId", pta.ProjectId));
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
            AllProjectAccesses.Clear();
            _allProjectAccesses = GetAllFromDB();
        }

        private List<ProjectAccess> GetAllFromDB()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM ProjectAccess", con);
                con.Open();
                var reader = command.ExecuteReader();

                var ptas = new List<ProjectAccess>();

                while (reader.Read())
                {
                    var pta = JsonConvert.DeserializeObject<ProjectAccess>((string)reader["AccessString"]);
                    pta.Id = (int)reader["Id"];
                    pta.AccountLogin = (string)reader["AccountLogin"];
                    pta.ProjectId = (int)reader["ProjectId"];
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
