using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers
{
    public class ProjectController : Controller
    {
        [Authorize]
        public ActionResult Index(int id)
        {
            var project = Logic.Logic.Instance.Project.Get(id);
            ViewBag.IsAdmin = Logic.Logic.Instance.AccountAccess.IsAdmin();

            if (project is null)
            {
                ViewBag.ErrorMessage = "Проект не найден";
                return RedirectToAction("Index", "Error");
            }

            var model = (ProjectModel)project;
            model.FillTaskList();
            //model.FillAccess(project);
            model.FillFilePaths();

            model.FillLogs();

            return View("IndexJNew", model);
        }

        [ControllerAccess]
        public ActionResult Create(int projectTypeId)
        {
            var model = new ProjectModel();

            ViewData["projectTypeId"] = projectTypeId;
            ViewData["projectTypeName"] = Logic.Logic.Instance.ProjectType.Get(projectTypeId)?.Name;

            model.DateEndFact = ModelHelper.MinSQLDate;

            //model.FillAccess(null);

            return View("CreateJNew", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                var id = Logic.Logic.Instance.Project.Create((Project)model);

                return RedirectToAction("Details", "ProjectType", new { id = model.ProjectTypeId });
            }

            ViewData["projectTypeId"] = model.ProjectTypeId;

            //model.FillAccess(null);

            return View("CreateJNew", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                Logic.Logic.Instance.Project.Update((Project)model);
                return RedirectToAction("Details", "ProjectType", new { id = model.ProjectTypeId });
            }
            else
            {
                //model.FillAccess(null);
                model.FillFilePaths();
                return View("EditJNew", model);
            }
        }

        [Authorize]
        [ControllerAccess]
        public ActionResult Edit(int id)
        {
            var project = Logic.Logic.Instance.Project.Get(id);

            var model = (ProjectModel)project;

            //model.FillAccess(project);
            model.FillFilePaths();

            return View("EditJNew", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Delete(int id, string projectTypeId)
        {
            var result = Logic.Logic.Instance.Project.Delete(id);

            int.TryParse(projectTypeId, out var projTypeIdInt);

            return RedirectToAction("Details", "ProjectType", new { id = projectTypeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Restore(int id, string projectTypeId)
        {
            var result = Logic.Logic.Instance.Project.Restore(id);

            if (result.Success)
            {
                return RedirectToAction("Details", "ProjectType", new { id = projectTypeId });
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult ChangeProjectType(int projectId)
        {
            var project = Logic.Logic.Instance.Project.Get(projectId);

            var model = (ProjectModel)project;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult ChangeProjectType(int projectId, int projectTypeId)
        {
                Logic.Logic.Instance.Project.ChangeProjectType(projectId, projectTypeId);
            

            return Index(projectId);
        }

        [HttpPost]        
        [Authorize]
        public int AddFilePath(int projectId)
        {
            return Logic.Logic.Instance.Project.CreateFilePath(projectId);
        }

        [HttpPost]
        [Authorize]
        public bool DeleteFilePath(int filePathId)
        {
            return Logic.Logic.Instance.Project.DeleteFilePath(filePathId).Success;
        }

        [HttpPost]
        [Authorize]
        public bool UpdateFilePath(int filePathId, string description, string path)
        {
            var filePath = new ProjectPathToFile(filePathId, 0, description, path);

            return Logic.Logic.Instance.Project.UpdateFilePath(filePath).Success;
        }

        #region CalendarPlan
        [Authorize]
        public ActionResult CalendarPlan(int id)
        {
            var model = (ProjectModel)Logic.Logic.Instance.Project.Get(id);
            model.CalendarPlan = Logic.Logic.Instance.Project.GetCalendarPlan(id);
            return View(model);
        }

        [Authorize]
        [ControllerAccess]
        public ActionResult CalendarPlanEdit(int id)
        {
            var model = (ProjectModel)Logic.Logic.Instance.Project.Get(id);

            model.CalendarPlan = Logic.Logic.Instance.Project.GetCalendarPlan(id);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public int AddCalendarPlanItem(int projectId)
        {
            return Logic.Logic.Instance.Project.CreateCalendarPlanItem(projectId);
        }

        [HttpPost]
        [Authorize]
        public bool DeleteCalendarPlanItem(int cpId)
        {
            return Logic.Logic.Instance.Project.DeleteCalendarPlanItem(cpId).Success;
        }

        [HttpPost]
        [Authorize]
        public bool UpdateCalendarPlanItem(int cpId, int stageNum, string workType, string deadlines)
        {
            var cpItem = new CalendarPlanItem(cpId, stageNum, workType, deadlines);

            return Logic.Logic.Instance.Project.UpdateCalendarPlanItem(cpItem).Success;
        }
        #endregion
    }
}