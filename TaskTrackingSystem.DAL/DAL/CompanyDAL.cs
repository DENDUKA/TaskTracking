using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class CompanyDAL : ICompanyDAL
    {
        private Dictionary<int, Company> companyCache = new Dictionary<int, Company>();
        private bool getAll = false;

        public Company Get(int id)
        {
            if (companyCache.ContainsKey(id))
            {
                return (Company)companyCache[id].Clone();
            }

            var company = GetFromDB(id);

            if (company is null)
                return null;

            companyCache.Add(id, company);

            return (Company)company.Clone();
        }

        public IEnumerable<Company> GetAll()
        {
            if (!getAll)
            {
                companyCache.Clear();

                foreach (var c in GetAllFromDB())
                {
                    companyCache.Add(c.Id, c);
                }

                getAll = true;
            }

            return companyCache.Select(status => (Company)status.Value.Clone()).ToList();
        }

        public void ClearCache()
        {
            companyCache.Clear();
            getAll = false;
        }

        public int Create(Company company)
        {
            ClearCache();

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Company(Form, Name, PhoneNumber, Email) values(@Form, @Name, @PhoneNumber, @Email) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Form", company.Form));
                command.Parameters.Add(new SqlParameter("@Name", company.Name));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", company.PhoneNumber ?? ""));
                command.Parameters.Add(new SqlParameter("@Email", company.Email ?? ""));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)(decimal)reader["SCOPE_IDENTITY"];
                }

                return 0;
            }
        }

        public OperationResult Update(Company company)
        {
            ClearCache();

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Company SET Form = @Form, Name = @Name, PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @id;", con);
                command.Parameters.Add(new SqlParameter("@id", company.Id));
                command.Parameters.Add(new SqlParameter("@Form", company.Form));
                command.Parameters.Add(new SqlParameter("@Name", company.Name));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", company.PhoneNumber ?? ""));
                command.Parameters.Add(new SqlParameter("@Email", company.Email ?? ""));

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

        private IEnumerable<Company> GetAllFromDB()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Company", con);
                con.Open();
                var reader = command.ExecuteReader();

                var companies = new List<Company>();

                while (reader.Read())
                {
                    companies.Add(ReadSQL(reader));
                }

                return companies;
            }
        }

        private Company GetFromDB(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Company WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));

                con.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadSQL(reader);
                }

                return null;
            }
        }

        private Company ReadSQL(SqlDataReader reader)
        {
            return new Company((int)reader["Id"], (string)reader["Name"], Shared.NullRead(reader["Email"]), Shared.NullRead(reader["PhoneNumber"]), (string)reader["Form"]);
        }

        public OperationResult Delete(int id)
        {
            ClearCache();
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Company WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
       
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
    }
}