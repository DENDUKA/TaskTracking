using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Shared.Entities.Access;
using TaskTrackingSystem.UI.Web.Models;

namespace TaskTrackingSystem.Controllers
{
    public class AccountAccessController : Controller
    {        
        [ControllerAccess]
        public ActionResult Index()
        {
            var model = AccountAccessModel.GetAll();

            return View("IndexJNew", model);
            //return View(model);
        }

        [ControllerAccess]
        public ActionResult ProjectAccess(int id)
        {
            var project = Logic.Logic.Instance.Project.Get(id);

            ViewBag.ProjectId = project.Id;
            ViewBag.ProjectName = project.Name;

            var model = AccountAccessModel.GetAll();

            return View(model);
        }

        #region ProjectAccess
        [HttpPost]
        [Authorize]        
        public bool UpdateProjectAccess(int id, string accountLogin, string[] data) 
        {
            var pa = new ProjectAccess(id, accountLogin, 0);

            var i = 0;
            foreach (var meta in AccountAccessModel.MetaDataPA)
            {
                var propertyInfo = pa.GetType().GetProperty(meta.FieldName);
                propertyInfo.SetValue(pa, bool.Parse(data[i++]));
            }

            return Logic.Logic.Instance.AccountAccess.ProjectEdit(pa);
        }

        [HttpPost]
        [Authorize]
        public bool DeleteProjectAccess(int paID)
        {
            return Logic.Logic.Instance.AccountAccess.ProjectDelete(paID);
        }

        [HttpPost]
        [Authorize]
        public int CreateProjectAccess(int paID, string login)
        {
            var pa = new ProjectAccess(0, login, paID);

            return Logic.Logic.Instance.AccountAccess.ProjectAdd(pa);
        }
        #endregion

        #region ProjectTypeAccess
        [HttpPost]
        [Authorize]
        public bool DeleteProjectTypeAccess(int ptaid)
        {
            return Logic.Logic.Instance.AccountAccess.ProjectTypeDelete(ptaid);
        }

        [HttpPost]
        [Authorize]
        public bool UpdateProjectTypeAccess(int id, int ptid, string[] data)
        {
            var pta = new ProjectTypeAccess(id, "", ptid);

            var i = 0;
            foreach (var meta in AccountAccessModel.MetaDataPTA)
            {
                var propertyInfo = pta.GetType().GetProperty(meta.FieldName);
                propertyInfo.SetValue(pta, bool.Parse(data[i++]));
            }

            return Logic.Logic.Instance.AccountAccess.ProjectTypeEdit(pta);
        }

        [HttpPost]
        [Authorize]
        public int CreateProjectTypeAccess(int ptID, string login)
        {
            var pta = new ProjectTypeAccess(0, login, ptID);

            var id = Logic.Logic.Instance.AccountAccess.ProjectTypeAdd(pta);

            return id;
        }
        #endregion

        #region AccountAccess
        [HttpPost]
        [Authorize]
        public bool UpdateAccountAccess(string accLogin, string[] data)
        {
            var aa = new AccountAccess(accLogin, null);

            var i = 0;
            foreach (var aaMeta in AccountAccessModel.MetaDataAA)
            {
                var propertyInfo = aa.GetType().GetProperty(aaMeta.FieldName);
                propertyInfo.SetValue(aa, bool.Parse(data[i++]));
            }

            return Logic.Logic.Instance.AccountAccess.AccountAccessEdit(aa);
        }
        #endregion
    }
}