using System.Collections.Generic;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.DAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class CompanyBLL
    {
        #region singleton
        public static CompanyBLL Instance { get; private set; }

        private readonly ICompanyDAL companyDAL;

        private CompanyBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    companyDAL = new CompanyDAL();
                    break;
                case "Local":
                    companyDAL = new CompanyLocalDAL();
                    break;
                case "EFDB":
                    companyDAL = new EFDAL.CompanyDAL();
                    break;
            }
        }

        static CompanyBLL()
        {
            Instance = new CompanyBLL();
        }

        internal void ClearCache()
        {
            companyDAL.ClearCache();
        }
        #endregion

        public OperationResult Delete(int id)
        {
            return companyDAL.Delete(id);
        }

        public int Create(Company company)
        {
            return companyDAL.Create(company);
        }

        public void Update(Company company)
        {
            companyDAL.Update(company);
        }

        public Company Get(int id)
        {
            return companyDAL.Get(id);
        }

        public IEnumerable<Company> GetAll()
        {
            return companyDAL.GetAll();
        }
    }
}
