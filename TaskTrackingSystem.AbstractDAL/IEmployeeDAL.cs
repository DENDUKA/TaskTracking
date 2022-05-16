using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface IEmployeeDAL
    {
        Employee Get(int id);
        OperationResult Delete(int id);
        int Create(Employee employee);
        OperationResult Update(Employee employee);
        List<Employee> GetAllForCompany(int companyId);
    }
}
