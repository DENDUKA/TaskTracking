using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        public int Create(Employee employee)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Employee(Name, PhoneNumber, AdditionalNumber, Email, CompanyId) " +
                    "values(@Name, @PhoneNumber, @AdditionalNumber, @Email, @CompanyId) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];", con);
                command.Parameters.Add(new SqlParameter("@Name", employee.Name));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(employee.PhoneNumber) ? "" : employee.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@AdditionalNumber", employee.AdditionalNumber is null ? (object)DBNull.Value : employee.AdditionalNumber));
                command.Parameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(employee.Email) ? "" : employee.Email));
                command.Parameters.Add(new SqlParameter("@CompanyId", employee.CompanyId));

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
                var command = new SqlCommand("DELETE FROM Employee WHERE id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();
                return new OperationResult(reader.Read());
            }
        }

        public Employee Get(int id)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id,Name,PhoneNumber,AdditionalNumber,Email,CompanyId FROM Employee WHERE Id = @id", con);
                command.Parameters.Add(new SqlParameter("@id", id));
                con.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return ReadEmployee(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Employee> GetAllForCompany(int companyId)
        {
            var employees = new List<Employee>();

            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand("SELECT Id,Name,PhoneNumber,AdditionalNumber,Email,CompanyId FROM Employee WHERE companyId = @companyId", con);
                command.Parameters.Add(new SqlParameter("@companyId", companyId));

                con.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(ReadEmployee(reader));
                }
            }

            return employees;
        }

        public OperationResult Update(Employee employee)
        {
            using (var con = new SqlConnection(Shared.ConnectionString))
            {
                var command = new SqlCommand($"UPDATE Employee SET Name = @Name, PhoneNumber = @PhoneNumber, AdditionalNumber = @AdditionalNumber, " +
                   $"Email = @Email, CompanyId = @CompanyId WHERE Id = @Id;", con);
                command.Parameters.Add(new SqlParameter("@Id", employee.Id));
                command.Parameters.Add(new SqlParameter("@Name", employee.Name));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(employee.PhoneNumber) ? "" : employee.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@AdditionalNumber", employee.AdditionalNumber is null ? (object)DBNull.Value : employee.AdditionalNumber));
                command.Parameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(employee.Email) ? "" : employee.Email));
                command.Parameters.Add(new SqlParameter("@CompanyId", employee.CompanyId));

                con.Open();
                return new OperationResult(command.ExecuteNonQuery() == 1);
            }
        }

        private Employee ReadEmployee(SqlDataReader reader)
        {
            var i = (int)reader["CompanyId"];

            return new Employee(
                        (int)reader["Id"],
                        (string)reader["Name"],
                        reader["PhoneNumber"] == DBNull.Value ? "" : (string)reader["PhoneNumber"],
                        reader["AdditionalNumber"] == DBNull.Value ? null : (int?)reader["AdditionalNumber"],
                        reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                        (int)reader["CompanyId"]
                        );
        }
    }
}