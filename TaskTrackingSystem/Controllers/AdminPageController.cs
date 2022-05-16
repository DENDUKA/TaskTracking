using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using TaskTrackingSystem.Logic;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers
{
    public class AdminPageController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View("IndexJNew");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Awards()
        {
            var model = ProjectModel.GetAllWithPremiumUnPaid();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsPaidAllProjects()
        {
            foreach (var p in ProjectModel.GetAllWithPremiumUnPaid())
            {
                ProjectBLL.Instance.MarkAsPaid(p.Id);
            }

            return RedirectToAction("Awards");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult AwardsExcel()
        {
            //Logic.Logic.Instance.LogEvent.Log.Debug("EvaluationProjectReport Started");
            var exelPath = Logic.Files.Exel.AwardsCalculate();

            //var projectType = Logic.Logic.Instance.ProjectType.Get(projectTypeId);

            var fileBytes = System.IO.File.ReadAllBytes(exelPath);
            var fileName = $"Премии.{DateTime.Now}.xlst";

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [Authorize(Roles = "admin")]
        public ActionResult PayList()
        {
            return View("PayList");
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength == 0)
                {
                    ViewBag.Message = "Что-то пошло не так";
                    return View("PayList");
                }

                var filePath = $"C:\\Server\\data\\Excel\\{Guid.NewGuid()}.xls";
                var model = new PayListModel();

                file.SaveAs(filePath);
                model.Parce(filePath);

                ViewBag.Message = "Успешно";
                return View("PayList", model);
            }

            catch
            {
                ViewBag.Message = "Что-то пошло не так";
                return View("PayList");
            }
        }

        public ActionResult PayListItemTable(string login, int year, int month)
        {
            var model = PayListBLL.Instance.GetForAccount(login,  year,  month);
            return PartialView("~/Views/Account/Partial/PayListItemTable.cshtml", model);

        }
    }
}