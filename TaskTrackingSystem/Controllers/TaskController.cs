using System;
using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers
{
    public class TaskController : Controller
    {
        [Authorize]
        [ControllerAccess]
        public ActionResult Edit(int id)
        {
            var task = Logic.Logic.Instance.Task.Get(id);

            if (task != null)
            {
                //Если задача удалена
                if (task.Status.Name.Equals(Status.Deleted))
                {
                    ViewBag.ErrorMessage = "Удаленная задача доступна только админу";
                    return RedirectToAction("Index", "Error");
                }

                var model = (TaskModel)task;

                return View("EditJNew", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Задача с таким Id не найдена";
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(TaskModel model)
        {

            if (ModelState.IsValid)
            {
                if (Logic.Logic.Instance.Task.Update((Task)model))
                {

                }
                else
                {

                }
            }
            else
            {
                var task = Logic.Logic.Instance.Task.Get(model.Id);


                return View("EditJNew", (TaskModel)task);
            }
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string projectId)
        {
            var result = Logic.Logic.Instance.Task.Delete(id);

            return RedirectToAction("Index", "Project", new { id = projectId });
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var task = Logic.Logic.Instance.Task.Get(id);

            if (task != null)
            {
                if (task.Status.Name.Equals(Status.Deleted) && !ModelHelper.IsCurrentUserAdmin())
                {
                    ViewBag.ErrorMessage = "Удаленная задача доступна только админу";                    
                    return RedirectToAction("Index", "Error");
                }

                var project = Logic.Logic.Instance.Project.Get(task.ProjectId);

                ViewData["ProjectName"] = project.Name;
                ViewData["ProjectId"] = project.Id;
                ViewData["ProjectTypeName"] = project.ProjectType.Name;
                ViewData["ProjectTypeId"] = project.ProjectType.Id;

                var model = (TaskModel)task;

                model.FillLogs();
                model.FillComments();

                return View("DetailsJNew", model);
            }
            else
            {
                ViewBag.ErrorMessage = "Задача с таким Id не найдена";
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Details(int id, int statusId)
        {
            Logic.Logic.Instance.Task.UpdateStatus(id, statusId);
            return Details(id);

            //TODO исправить метод контроллера
        }

        [Authorize]
        [ControllerAccess]
        public ActionResult Create(int projectId)
        {
            var model = new TaskModel();

            var project = Logic.Logic.Instance.Project.Get(projectId);

            ViewData.Add("projectName", project.Name);
            ViewData.Add("projectId", projectId);

            return View("CreateJNew", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                var taskId = Logic.Logic.Instance.Task.Add((Task)model);

                if (taskId != 0)
                {
                    return RedirectToAction("Details", new { id = taskId });
                }
            }

            var project = Logic.Logic.Instance.Project.Get(model.ProjectId);

            ViewData.Add("projectId", model.ProjectId);

            return View("CreateJNew", model);
        }

        [Authorize]
        public ActionResult CreateForSysAdmin()
        {
            var model = new TaskModel();
            var settings = Logic.Logic.Instance.Settings.Get();

            ViewData.Add("projectName", Logic.Logic.Instance.Project.Get(settings.ProjectIdForSystemAdmin)?.Name);

            return View("CreateForSysAdminJNew", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateForSysAdmin(TaskModel model)
        {
            var settings = Logic.Logic.Instance.Settings.Get();

            model.AccountLogin = settings.SystemAdminLogin;
            model.ProjectId = settings.ProjectIdForSystemAdmin;
            model.DateStart = DateTime.Today;
            model.DateEnd = DateTime.Today.AddDays(2);

            if (ModelState.IsValidField("Name"))
            {
                var taskId = Logic.Logic.Instance.Task.Add((Task)model);
                return RedirectToAction("Details", new { id = taskId });
            }

            return View("CreateForSysAdminJNew", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ControllerAccess]
        public ActionResult Restore(int id)
        {
            var result = Logic.Logic.Instance.Task.Restore(id);

            if (result.Success)
            {                
                return RedirectToAction("Details", new { id });
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return RedirectToAction("Index", "Error");
            }
        }

        [Authorize]
        public ActionResult Pocker(int id)
        {
            var task = Logic.Logic.Instance.Task.Get(id);
            if (task != null)
            {
                var model = (TaskModel)task;
            }

            return null;
        }

        #region partialViews

        public ActionResult _PeopleBlock()
        {
            return PartialView();
        }

        public ActionResult _DatesBlock()
        {
            return PartialView();
        }

        public ActionResult _DetailsBlock()
        {
            return PartialView();
        }

        public ActionResult _ActivityBlock()
        {
            return PartialView();
        }

        #endregion
    }
}