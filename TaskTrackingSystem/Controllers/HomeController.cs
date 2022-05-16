using System.Web.Mvc;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Shared.Entities;
using System;
using Exceptionless;

namespace TaskTrackingSystem.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsAdmin = Logic.Logic.Instance.AccountAccess.IsAdmin();


            return View("IndexJNew");
        }

        [ControllerAccess]
        public ActionResult PatchNotes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Admin)]
        public ActionResult ClearCache()
        {
            Logic.Logic.Instance.Cache.Clear();

            return RedirectToAction("Index", "AdminPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Admin)]
        public ActionResult SomeAction()
        {
            new Exception("Это моё исключение").ToExceptionless().Submit();
        
        //{
        //    Logic.Logic.Instance.LogEvent.Log.Debug("SomeAction Start");


        //    var StatusColor = new Dictionary<int, int>()
        //{
        //    { 1 , System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Khaki) },
        //    { 2 , System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) },
        //    { 3 , System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red) },
        //    };


        //    var projectTypeId = 1;

        //    var ObjExcel = new Microsoft.Office.Interop.Excel.Application();

        //    Logic.Logic.Instance.LogEvent.Log.Debug("1");

        //    var ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);

        //    Logic.Logic.Instance.LogEvent.Log.Debug("2");

        //    var ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];

        //    Logic.Logic.Instance.LogEvent.Log.Debug("3");



        //    var projectType = Logic.Logic.Instance.ProjectType.Get(projectTypeId);
        //    var projects = Logic.Logic.Instance.Project.GetAllForProjectType(projectType.Id).FindAll(p => p.Status.Id != Status.DeletedId).OrderByDescending(p => p.Status.Id);

        //    Logic.Logic.Instance.LogEvent.Log.Debug("4");

        //    ObjWorkSheet.Columns[1].ColumnWidth = 4;
        //    ObjWorkSheet.Columns[2].ColumnWidth = 20;
        //    ObjWorkSheet.Columns[3].ColumnWidth = 12;
        //    ObjWorkSheet.Columns[4].ColumnWidth = 12;
        //    ObjWorkSheet.Columns[5].ColumnWidth = 16;
        //    ObjWorkSheet.Columns[6].ColumnWidth = 16;
        //    ObjWorkSheet.Columns[7].ColumnWidth = 16;

        //    Logic.Logic.Instance.LogEvent.Log.Debug("5");

        //    ObjWorkSheet.Cells[1, 1] = "Статус";
        //    ObjWorkSheet.Cells[1, 2] = "Название проекта";
        //    ObjWorkSheet.Cells[1, 3] = "Дата начала";
        //    ObjWorkSheet.Cells[1, 4] = "Дата завершения";
        //    ObjWorkSheet.Cells[1, 5] = "Стоимость";
        //    ObjWorkSheet.Cells[1, 6] = "Все оценки";
        //    ObjWorkSheet.Cells[1, 7] = "Задач без оценок";

        //    Logic.Logic.Instance.LogEvent.Log.Debug("6");

        //    var nextAccountCol = 8;
        //    var nextProjectRow = 2;

        //    var AccountCell = new Dictionary<string, int>();

        //    Logic.Logic.Instance.LogEvent.Log.Debug("7");

        //    Logic.Logic.Instance.LogEvent.Log.Debug("Excel Column Created");


        //    foreach (var project in projects)
        //    {
        //        var tasks = Logic.Logic.Instance.Task.GetAllForProject(project.Id);

        //        ObjWorkSheet.Cells[nextProjectRow, 1].Interior.Color = StatusColor[project.Status.Id];
        //        ObjWorkSheet.Cells[nextProjectRow, 1] = project.Status.Name;
        //        ObjWorkSheet.Cells[nextProjectRow, 2] = project.Name;
        //        ObjWorkSheet.Cells[nextProjectRow, 3] = project.DateStart;
        //        ObjWorkSheet.Cells[nextProjectRow, 4] = project.DateEnd;
        //        ObjWorkSheet.Cells[nextProjectRow, 5] = project.Cost;
        //        ObjWorkSheet.Cells[nextProjectRow, 6] = tasks.Sum(x => x.StoryPoints);
        //        ObjWorkSheet.Cells[nextProjectRow, 7] = tasks.FindAll(x => x.StoryPoints == 0 && x.Status.Id != Status.DeletedId).Count;


        //        var r = tasks.GroupBy(u => u.Account.Name).Select(grp => grp.First()).ToList();


        //        foreach (var acc in tasks.GroupBy(u => u.Account.Login).Select(grp => grp.First().Account))
        //        {
        //            var pointsDone = tasks.FindAll(t => t.Status.Id == Status.DoneId && t.Account.Login.Equals(acc.Login)).Sum(m => m.StoryPoints);

        //            if (!AccountCell.ContainsKey(acc.Name))
        //            {
        //                ObjWorkSheet.Cells[1, nextAccountCol] = acc.Name;
        //                ObjWorkSheet.Columns[nextAccountCol].ColumnWidth = 16;
        //                AccountCell.Add(acc.Name, nextAccountCol++);
        //            }
        //            ObjWorkSheet.Cells[nextProjectRow, AccountCell[acc.Name]] = pointsDone;
        //        }

        //        nextProjectRow++;
        //    }

        //    Logic.Logic.Instance.LogEvent.Log.Debug("Excel Column Filled");


        //    var filePath = $"E:\\Server\\data\\Excel\\{Guid.NewGuid()}.xlsx";

        //    Logic.Logic.Instance.LogEvent.Log.Debug($"filePath {filePath}");

        //    ObjWorkBook.SaveAs(filePath);

        //    try
        //    {
        //        Logic.Logic.Instance.LogEvent.Log.Debug("ObjWorkBook.SaveAs Start");
        //        ObjWorkBook.SaveAs(filePath);
        //        Logic.Logic.Instance.LogEvent.Log.Debug("ObjWorkBook.SaveAs End");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logic.Logic.Instance.LogEvent.Log.Error($"ObjWorkBook.SaveAs Exception {ex.Message}");
        //        Logic.Logic.Instance.LogEvent.Log.Error(ex);
        //    }
        //    finally
        //    {
        //        Logic.Logic.Instance.LogEvent.Log.Debug("finally ObjWorkBook.Close start");
        //        ObjWorkBook.Close();
        //        Logic.Logic.Instance.LogEvent.Log.Debug("finally ObjWorkBook.Close end");
        //    }

        //    Logic.Logic.Instance.LogEvent.Log.Debug(filePath);
            




        //    //string path = @"E:\AmberLightEddison.xml";

        //    //var xml = XElement.Load(path);

        //    //var t = xml.Element("channel");
        //    //var tt = t.Elements("item");

        //    //List<Task> tasks = new List<Task>();

        //    //foreach (var x in tt)
        //    //{
        //    //    int statusId = 3;

        //    //    if (x.Element("resolution").Value.Equals("Unresolved"))
        //    //    {
        //    //        statusId = 1;
        //    //    }
        //    //    else if (x.Element("resolution").Value.Equals("Fixed"))
        //    //    {
        //    //        statusId = 3;
        //    //    }
        //    //    else if (x.Element("resolution").Value.Equals("Closed"))
        //    //    {
        //    //        statusId = 3;
        //    //    }
        //    //    else if (x.Element("resolution").Value.Equals("Resolved"))
        //    //    {
        //    //        statusId = 3;
        //    //    }
        //    //    else if (x.Element("resolution").Value.Equals("Open"))
        //    //    {
        //    //        statusId = 2;
        //    //    }



        //    //    string comments = "";


        //    //    var commentsElement = x.Element("comments");
        //    //    if (commentsElement != null)
        //    //        foreach (var comment in commentsElement.Elements("comment"))
        //    //        {
        //    //            comments += comment.Value + "\n";
        //    //        }




        //    //    string description = x.Element("description").Value + (string.IsNullOrEmpty(comments) ? "" : "\n\n Комментарии :\n" + comments);


        //    //    string[] tagsDel = new string[] { "<p>", "</p>", "</li>", "<li>", "<ul>", "</ul>", "</a>", "<a>", "<br/>", "</blockquote>", "<blockquote>", "<ol>", "</ol>", "<b>", "</b>", "<h4>", "</h4>" };

        //    //    foreach (var del in tagsDel)
        //    //    {
        //    //        description = description.Replace(del, "");
        //    //    }



        //    //    Console.WriteLine("---------------------");
        //    //    Console.WriteLine(description);

        //    //    tasks.Add(new Task(0, x.Element("title").Value, DateTime.Parse(x.Element("created").Value), DateTime.Parse(x.Element("updated").Value), new Status(statusId, ""), 16216, "",
        //    //        new AccountBase("DENDUKA", "DENDUKA"), description, new AccountBase("DENDUKA", "DENDUKA")));




        //    //}


        //    //foreach (var taskk in tasks)
        //    //{
        //    //    try
        //    //    {
        //    //        Logic.Logic.Instance.Task.Add(taskk);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {

        //    //    }
        //    //}

            return RedirectToAction("Index", "AdminPage");
        }
    }
}