using System;
using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class ProjectTypeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View("IndexJNew", ProjectTypeModel.GetAll());
        }
        
        [Authorize]
        public ActionResult Details(int id)
        {
            var model = (ProjectTypeModel)Logic.Logic.Instance.ProjectType.Get(id);
            model.FillProjectList();
            model.FillAccess();

            return View("DetailsJNew", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public FileResult Exel(int projectTypeId,DateTime? excelStart, DateTime? excelEnd)
        {
            //Logic.Logic.Instance.LogEvent.Log.Debug("EvaluationProjectReport Started");
            var exelPath = Logic.Files.Exel.EvaluationProjectReport(projectTypeId, excelStart, excelEnd);
            //Logic.Logic.Instance.LogEvent.Log.Debug("EvaluationProjectReport Ended");

            var projectType = Logic.Logic.Instance.ProjectType.Get(projectTypeId);

            var fileBytes = System.IO.File.ReadAllBytes(exelPath);
            var fileName = $"{projectType.Name}.{DateTime.Now}.xlst";

            //Logic.Logic.Instance.LogEvent.Log.Debug($"File ready {exelPath}");

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}