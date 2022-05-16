using System;
using System.Web.Mvc;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Controllers
{
    public class CommentController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int taskid, string comment)
        {
            Logic.Logic.Instance.Comment.Add(new Comment(0, taskid, "", DateTime.Now, comment));
            return RedirectToAction("Details", "Task", new { id = taskid });
        }
    }
}