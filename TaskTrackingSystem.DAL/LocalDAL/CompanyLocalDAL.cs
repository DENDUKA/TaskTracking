using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.DAL
{
    public class CompanyLocalDAL : ICompanyDAL
    {
        public void ClearCache()
        {
            
        }

        public int Create(Company company)
        {
            company.Id = LocalStorage.Companies.Keys.Max() +1;
            LocalStorage.Companies.Add(company.Id, company);
            return company.Id;
        }

        public OperationResult Delete(int id)
        {
            return new OperationResult(LocalStorage.Companies.Remove(id));
        }

        public Company Get(int id)
        {
            return LocalStorage.Companies[id];
        }

        public IEnumerable<Company> GetAll()
        {
            return LocalStorage.Companies.Values.ToList();
        }

        public OperationResult Update(Company company)
        {
            LocalStorage.Companies[company.Id] = company;
            return new OperationResult(true);
        }
    }
}