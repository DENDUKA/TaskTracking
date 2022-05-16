using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class EmployeeBLL
    {
        #region singleton
        public static EmployeeBLL Instance { get; private set; }

        private readonly IEmployeeDAL employeeDAL;

        private EmployeeBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    employeeDAL = new EmployeeDAL();
                    break;
                case "Local":
                    employeeDAL = new EmployeeLocalDAL();
                    break;
                case "EFDB":
                    employeeDAL = new EFDAL.EmployeeDAL();
                    break;
            }
        }

        static EmployeeBLL()
        {
            Instance = new EmployeeBLL();
        }
        #endregion

        public Employee Get(int id)
        {
            return employeeDAL.Get(id);
        }
        public OperationResult Delete(int id)
        {
            return employeeDAL.Delete(id);
        }
        public int Create(Employee employee)
        {
            return employeeDAL.Create(employee);
        }
        public OperationResult Update(Employee employee)
        {
            return employeeDAL.Update(employee);
        }
        public List<Employee> GetAllForCompany(int companyId)
        {
            return employeeDAL.GetAllForCompany(companyId);
        }
    }
}