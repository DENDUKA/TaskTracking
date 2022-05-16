using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ICompanyDAL
    {
        Company Get(int id);
        IEnumerable<Company> GetAll();
        int Create(Company company);
        OperationResult Update(Company company);
        void ClearCache();
        OperationResult Delete(int id);
    }
}
