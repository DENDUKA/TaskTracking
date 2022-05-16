using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class StatusDAL : IStatusDAL
    {
        private Dictionary<int, Status> statusCache = new Dictionary<int, Status>();
        private bool getAll = false;


        public Status Get(int id)
        {
            if (statusCache.ContainsKey(id))
            {
                return (Status)statusCache[id].Clone();
            }

            var status = GetFromDB(id);

            if (status is null)
                return null;

            statusCache.Add(id, status);

            return (Status)status.Clone();
        }

        private Status GetFromDB(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, Name FROM Status WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Status((int)reader["Id"], (string)reader["Name"]);
                }

                return null;
            }
        }

        public List<Status> GetAll()
        {
            if (!getAll)
            {
                statusCache.Clear();

                foreach (var s in GetAllFromDB())
                {
                    statusCache.Add(s.Id, s);
                }

                getAll = true;
            }

            return statusCache.Select(status => (Status)status.Value.Clone()).ToList();
        }

        private List<Status> GetAllFromDB()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id, Name FROM Status", con);
                con.Open();
                var reader = command.ExecuteReader();

                var statuses = new List<Status>();

                while (reader.Read())
                {
                    statuses.Add(new Status((int)reader["Id"], (string)reader["Name"]));
                }

                return statuses;
            }
        }

        public void ClearCache()
        {
            statusCache.Clear();
            getAll = false;
        }
    }
}
