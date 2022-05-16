using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers
{
    public class CompanyController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = CompanyModel.GetAll();

            return View("IndexJNew", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Delete(int id)
        {
            Logic.Logic.Instance.Company.Delete(id);
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var model = (CompanyModel)Logic.Logic.Instance.Company.Get(id);

            return View("DetailsJNew", model);
        }
        [ControllerAccess]
        public ActionResult Edit(int id)
        {
            var model = (CompanyModel)Logic.Logic.Instance.Company.Get(id);

            return View("EditJNew", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Edit(CompanyModel model)
        {
            if (ModelState.IsValid)
            {
                Logic.Logic.Instance.Company.Update((Company)model);
                return RedirectToAction("Index");
            }

            return View("EditJNew", model);
        }
        [Authorize]
        [ControllerAccess]
        public ActionResult Create()
        {
            var model = new CompanyModel();

            return View("CreateJNew", model);
        }
        [HttpPost]
        [Authorize]
        [ControllerAccess]
        public ActionResult Create(CompanyModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Logic.Logic.Instance.Company.Create((Company)model);
                return RedirectToAction("Details", new { id = model.Id });
            }

            return View("CreateJNew", model);
        }
    }
}