using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using TaskTrackingSystem.AbstractDAL.Access;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.DAL.DAL.Access
{
    public class AccountAccessDAL : IAccountAccessDAL
    {

        private List<AccountAccess> _allAccountAccesses = new List<AccountAccess>();
        private bool cacheIsFilled = false;
        private List<AccountAccess> AllAccountAccesses
        {
            get
            {
                if (!cacheIsFilled)
                {
                    UpdateCache();
                }
                return _allAccountAccesses;
            }
        }

        public bool Add(AccountAccess aa)
        {
            cacheIsFilled = false;

            var accessString = JsonConvert.SerializeObject(aa);

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO AccountAccess(Login, AccessString) values(@Login, @AccessString)", con);
                command.Parameters.Add(new SqlParameter("@Login", aa.Login));
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

        public bool Edit(AccountAccess aa)
        {
            cacheIsFilled = false;

            var accessString = JsonConvert.SerializeObject(aa);

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE AccountAccess SET AccessString = @AccessString WHERE Login = @Login;", con);
                command.Parameters.Add(new SqlParameter("@AccessString", accessString));
                command.Parameters.Add(new SqlParameter("@Login", aa.Login));

                con.Open();
                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool Delete(string login)
        {
            cacheIsFilled = false;

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM AccountAccess WHERE Login = @Login", con);
                command.Parameters.Add(new SqlParameter("@Login", login));

                con.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public AccountAccess Get(string login, int projectId = 0)
        {
            var aa = AllAccountAccesses.Find(acc => acc.Login.Equals(login));

            if (projectId != 0)
            {
                aa.ProjectAccesses = aa.ProjectAccesses.FindAll(pa => pa.ProjectId == projectId);
            }

            return aa;
        }        

        public List<AccountAccess> GetAll()
        {
            return AllAccountAccesses;
        }

        public void UpdateCache()
        {
            cacheIsFilled = true;
            AllAccountAccesses.Clear();
            _allAccountAccesses = GetAllFromDB();
            
        }

        private List<AccountAccess> GetAllFromDB()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM AccountAccess", con);
                con.Open();
                var reader = command.ExecuteReader();

                var ptas = new List<AccountAccess>();

                while (reader.Read())
                {
                    var aa = JsonConvert.DeserializeObject<AccountAccess>((string)reader["AccessString"]);
                    aa.Login = (string)reader["Login"];

                    ptas.Add(aa);
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
