using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class EmployeeLocalDAL : IEmployeeDAL
    {
        public int Create(Employee employee)
        {
            employee.Id = LocalStorage.Employees.Keys.Max() + 1;
            LocalStorage.Employees.Add(employee.Id, employee);
            return employee.Id;
        }

        public OperationResult Delete(int id)
        {
            return new OperationResult(LocalStorage.Employees.Remove(id));
        }

        public Employee Get(int id)
        {
            return LocalStorage.Employees[id];
        }

        public List<Employee> GetAllForCompany(int companyId)
        {
            return LocalStorage.Employees.Values.ToList().FindAll(x => x.CompanyId == companyId);
        }

        public OperationResult Update(Employee employee)
        {
            LocalStorage.Employees[employee.Id] = employee;
            return new OperationResult(true);
        }
    }
}