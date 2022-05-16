using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        public int Create(Employee employee)
        {
            using (var db = new EmployeeContext())
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return employee.Id;
            }
        }

        public OperationResult Delete(int id)
        {
            using (var db = new EmployeeContext())
            {
                var count = db.Employees
                     .Where(x => x.Id == id)
                     .Delete();

                return new OperationResult(count > 0);
            }
        }

        public Employee Get(int id)
        {
            using (var db = new EmployeeContext())
            {
                return db.Employees
                     .Where(x => x.Id == id)
                     .FirstOrDefault();
            }
        }

        public List<Employee> GetAllForCompany(int companyId)
        {
            using (var db = new EmployeeContext())
            {
                return db.Employees
                     .Where(x => x.CompanyId == companyId)
                     .ToList();
            }
        }

        public OperationResult Update(Employee employee)
        {
            using (var db = new EmployeeContext())
            {
                db.Employees.AddOrUpdate(employee);
                db.SaveChanges();
            }

            return new OperationResult(true);
        }
    }
}