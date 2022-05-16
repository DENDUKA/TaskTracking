using System;
using System.Web.Mvc;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.UI.Web.Controllers;

namespace TaskTrackingSystem.Controllers
{
    public class TimeTrackController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = TimeTrackModel.GetAll();

            return View(model);
        }

        [Authorize]
        public ActionResult Create(string login = null)
        {
            var model = new TimeTrackModel
            {
                TimeString = "0h 0m",
                AccountLogin = login,
            };

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TimeTrackModel model)
        {
            if (ModelState.IsValid)
            {
                model.Description = model.Description ?? "";

                model.DateStart = model.DateStart.AddMinutes((model.DateStartTime.Hour * 60) + model.DateStartTime.Minute);
                model.DateEnd = model.DateEnd.AddMinutes((model.DateEndTime.Hour * 60) + model.DateEndTime.Minute);

                var ttId = Logic.Logic.Instance.TimeTrack.Create((TimeTrack)model);

                if (ttId != 0)
                {
                    return RedirectToAction("Details", "Account", new { id = model.AccountLogin});
                }
            }

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string login = null)
        {
            if (id != 0)
            {
                Logic.Logic.Instance.TimeTrack.Delete(id);
            }

            if (login != null)
            {
                return RedirectToAction("Details", "Account", new { id = login });
            }

            return RedirectToAction("Index");
        }
    }
}