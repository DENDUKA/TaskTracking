using System;
using System.Linq;
using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class CalendarController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = new CalendarModel();
            return View("IndexJNew", model);
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetDiaryEvents(DateTime start, DateTime end)
        {
            var calendarEvents = CalendarModel.LoadAllAppointmentsInDateRange(start, end).Select(x => new
            {
                id = x.ID,
                title = x.Title,
                start = x.StartDateString,
                end = x.EndDateString,
                color = x.StatusColor,
                className = x.ClassName,
                someKey = x.SomeImportantKeyID,
                allDay = x.AllDay,
                startEditable = x.StartEditable,
            }).ToList();

            return Json(calendarEvents, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ControllerAccess]
        public void EventChangeDateStart(int eventId, string dateStart, string dateEnd)
        {
            CalendarModel.UpdateDateStart(eventId, dateStart, dateEnd);
        }

        [HttpPost]
        [ControllerAccess]
        public int CreateEvent(string name, string dateStart, string dateEnd)
        {            
            return CalendarModel.CreateDiaryEvent(name, dateStart, dateEnd);
        }

        [HttpPost]
        [ControllerAccess]
        public bool DeleteEvent(int id)
        {
            return CalendarModel.DeleteDiaryEvent(id);
        }
    }
}