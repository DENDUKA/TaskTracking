using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
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

            using (var db = new CompanyContext())
            {
                db.Companys.Add(company);
                db.SaveChanges();
                return company.Id;
            }
        }

        public OperationResult Update(Company company)
        {
            ClearCache();

            using (var db = new CompanyContext())
            {
                db.Companys.AddOrUpdate(company);
                db.SaveChanges();
            }

            return new OperationResult(true);
        }

        private List<Company> GetAllFromDB()
        {
            using (var db = new CompanyContext())
            {
                return db.Companys.ToList();
            }
        }

        private Company GetFromDB(int id)
        {
            using (var db = new CompanyContext())
            {
                return db.Companys.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public OperationResult Delete(int id)
        {
            ClearCache();

            using (var db = new CompanyContext())
            {
                var count = db.Companys
                     .Where(x => x.Id == id)
                     .Delete();

                return new OperationResult(count > 0);
            }
        }
    }
}