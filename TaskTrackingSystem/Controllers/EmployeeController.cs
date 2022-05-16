using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize]
        public ActionResult Details(int id)
        {
            var model = (EmployeeModel)Logic.Logic.Instance.Employee.Get(id);

            model.Company = Logic.Logic.Instance.Company.Get(model.CompanyId);

            return View("DetailsJNew", model);
            //return View(model);
        }

        [ControllerAccess]
        public ActionResult Edit(int id)
        {
            var model = (EmployeeModel)Logic.Logic.Instance.Employee.Get(id);

            model.Company = Logic.Logic.Instance.Company.Get(model.CompanyId);

            return View("EditJNew", model);
            //return View(model);
        }

        [HttpPost]
        [ControllerAccess]
        public ActionResult Edit(EmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                Logic.Logic.Instance.Employee.Update((Employee)model);
                return RedirectToAction("Details", new { id = model.Id });
            }

            model.Company = Logic.Logic.Instance.Company.Get(model.CompanyId);

            return View("EditJNew", model);
            //return View(model);
        }

        [Authorize]
        public ActionResult Create(int companyId)
        {
            var model = new EmployeeModel
            {
                CompanyId = companyId,
                Company = Logic.Logic.Instance.Company.Get(companyId),
            };

            return View("CreateJNew", model);
            //return View(model);
        }

        [HttpPost]
        public ActionResult Create(EmployeeModel model)
        {
            var employee = (Employee)model;

            if (ModelState.IsValid)
            {
                model.Id = Logic.Logic.Instance.Employee.Create((Employee)model);
                return RedirectToAction("Details", new { id = model.Id });
            }

            model.Company = Logic.Logic.Instance.Company.Get(model.CompanyId);

            return View("CreateJNew", model);
            //return View(model);
        }

        [HttpPost]
        [ControllerAccess]
        public ActionResult Delete(int id)
        {
            var employee = Logic.Logic.Instance.Employee.Get(id);

            Logic.Logic.Instance.Employee.Delete(id);

            return RedirectToAction("Details", "Company", new { Id = employee.CompanyId });
        }
    }
}