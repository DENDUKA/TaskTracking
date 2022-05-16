 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class AccountDAL : IAccountDAL
    {
        private Dictionary<string, Account> accountCache = new Dictionary<string, Account>();
        private bool getAll = false;

        public Account Get(string login)
        {
            if (accountCache.ContainsKey(login))
            {
                return (Account)accountCache[login].Clone();
            }

            var account = GetFromDB(login);

            if (account is null)
                return null;

            accountCache.Add(login, account);

            return (Account)account.Clone();
        }

        public Account GetFromDB(string login)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Account WHERE Login=@login", con);
                command.Parameters.Add(new SqlParameter("@login", login));
                con.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Account((string)reader["Name"], (string)reader["login"], (string)reader["Password"], (string)reader["Role"], Shared.NotNullRead(reader["Email"]), Shared.NotNullRead(reader["PCName"]), (bool)reader["IsHidden"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Account> GetAll()
        {
            if (!getAll)
            {
                accountCache.Clear();

                foreach (var a in GetAllFromDB())
                {
                    accountCache.Add(a.Login, a);
                }

                getAll = true;
            }

            return accountCache.Select(status => (Account)status.Value.Clone()).ToList();
        }

        public List<Account> GetAllFromDB()
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Account", con);
                con.Open();
                var reader = command.ExecuteReader();

                var accs = new List<Account>();

                while (reader.Read())
                {
                    accs.Add(new Account((string)reader["Name"], (string)reader["Login"], (string)reader["Password"], (string)reader["Role"], Shared.NotNullRead(reader["Email"]), Shared.NotNullRead(reader["PCName"]), (bool)reader["IsHidden"]));
                }
                return accs;
            }
        }

        public void ClearCache()
        {
            accountCache.Clear();
            getAll = false;
        }

        public int Create(Account account)
        {
            ClearCache();
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Account(Login, Password, Name, Role, Email, PCName, IsHidden) values(@Login, @Password, @Name, @Role, @Email, @PCName, @ishidden) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Login", account.Login));
                command.Parameters.Add(new SqlParameter("@Password", account.Password));
                command.Parameters.Add(new SqlParameter("@Name", account.Name));
                command.Parameters.Add(new SqlParameter("@Role", account.Role));
                if (account.Email is null)
                    command.Parameters.Add(new SqlParameter("@Email", DBNull.Value));
                else
                    command.Parameters.Add(new SqlParameter("@Email", account.Email));
                command.Parameters.Add(new SqlParameter("@PCName", account.PCName ?? ""));
                command.Parameters.Add(new SqlParameter("@ishidden", account.IsHidden));


                con.Open();
                try
                {
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return (int)(decimal)reader["SCOPE_IDENTITY"];
                    }
                }
                catch (Exception ex)
                {
                }

                return 0;
            }
        }

        public void Delete(string login)
        {
            ClearCache();
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("DELETE FROM Account WHERE login = @login", con);
                command.Parameters.Add(new SqlParameter("@login", login));

                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool Update(Account acc)
        {
            ClearCache();
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("UPDATE Account SET Password = @password, Name = @name, Email = @email, PCName = @pcname, IsHidden = @ishidden WHERE Login = @login;", con);
                command.Parameters.Add(new SqlParameter("@login", acc.Login));
                command.Parameters.Add(new SqlParameter("@password", acc.Password));
                command.Parameters.Add(new SqlParameter("@name", acc.Name));
                command.Parameters.Add(new SqlParameter("@email", acc.Email));
                command.Parameters.Add(new SqlParameter("@pcname", acc.PCName));
                command.Parameters.Add(new SqlParameter("@ishidden", acc.IsHidden));

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
    }
}