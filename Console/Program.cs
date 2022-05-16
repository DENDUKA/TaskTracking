using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Logic;
using TaskTrackingSystem.Logic.Network;
using TaskTrackingSystem.Shared.Entities;

namespace ConsoleP
{
    public class Program
    {
        static void Main(string[] args)
        {

            //PCOnline.RunScan(true);

            //PCOnline.FUNCTION();



            //var x1 = PCOnline.ConvertIpToMAC("192.168.31.1");
            //var x2 = PCOnline.ConvertIpToMAC("192.168.31.30");
            //var x3 = PCOnline.ConvertIpToMAC("192.168.31.123");
            //var x4 = PCOnline.ConvertIpToMAC("192.168.31.138");


           // var list = PCOnline.RunScan(true);
            //PCOnline.WOL("18:C0:4D:2A:23:BC");
           PCOnline.Instance.WakeOnLan("18-C0-4D-2A-23-BC");


          

            Console.ReadKey();


            //var pts = TaskTrackingSystem.Logic.ProjectTypeBLL.Instance.GetAll();
            //var activeProjects = new List<Project>();
            //var activeTasks = new List<Task>();

            //foreach (var pt in pts)
            //{
            //    activeProjects.AddRange(ProjectBLL.Instance.GetAllForProjectType(pt.Id).Where(x => x.StatusId == Status.InWorkId || x.StatusId == Status.PostpondedId));                
            //}

            //foreach (var p in activeProjects)
            //{
            //    activeTasks.AddRange(TaskBLL.Instance.GetAllForProject(p.Id).Where(x => (x.DateEnd >= DateTime.Today) && (x.StatusId == Status.InWorkId || x.StatusId == Status.PostpondedId)));
            //}

            //foreach (var t in activeTasks)
            //{
            //    var r = (t.DateEnd - DateTime.Now.Date).TotalSeconds / (t.DateEnd - t.DateStart).TotalSeconds;
            //    Console.WriteLine($"{t.DateStart}  {t.DateEnd} {r}");
            //}

            //var i = 0;


            ////TaskTrackingSystem.Logic.TaskBLL.Instance.GetAllForProject();









            //using (var db = new AccountContext())
            //{
            //    var accounts = db.Accounts//.Where(a => a.Login == "DENDUKA")
            //        .Include(x => x.Project)
            //        .Include(x => x.AccountAccess)
            //        .Include(x => x.Comment)
            //        .Include(x => x.ProjectAccess)
            //        .Include(x => x.ProjectTypeAccess)
            //        .Include(x => x.TimeTrack)
            //        .Include(x => x.Task)
            //        .Include(x => x.AsReporter)
            //        .ToList();
            //}

            //using (var db = new AccountContext())
            //{
            //    var accounts = db.Accounts.Where(a => a.Login == "DENDUKA")
            //        .Include(x => x.Project)
            //        .Include(x => x.AccountAccess)
            //        .Include(x => x.Comment)
            //        .Include(x => x.ProjectAccess)
            //        .Include(x => x.ProjectTypeAccess)
            //        .Include(x => x.TimeTrack)
            //        .Include(x => x.Task)
            //        .Include(x => x.AsReporter)
            //        .ToList();
            //}

            //using (var db = new SettingContext())
            //{
            //    var Settings = db.Settings.First();
            //}

            //using (var db = new AccountAccessContext())
            //{
            //    var AccountAccess = db.AccountAccesses
            //        .Where(x => x.Login == "DENDUKA")
            //        .Include(x => x.Account)
            //        .ToList();
            //}

            //using (var db = new ActionLogsContext())
            //{
            //    var actionLogs = db.ActionLogs
            //        .Where(x => x.TypeId == 3067)
            //        .Where(x => x.Type == "Task")
            //        .Include(x => x.Account)
            //        .ToList();
            //}

            //using (var db = new CalendarDiaryEventContext())
            //{
            //    var cde = db.CalendarDEs.ToList();
            //}

            //using (var db = new CalendarPlanContext())
            //{
            //    var cpc = db.CalendarPlans
            //        .Include(x => x.Project)
            //        .ToList();
            //}

            //using (var db = new CommentContext())
            //{
            //    var cms = db.Comments
            //        .Include(x => x.Account)
            //        .Include(x => x.Task)
            //        .ToList();
            //}

            //using (var db = new CompanyContext())
            //{
            //    var cs = db.Companys
            //        .Include(x => x.Employee)
            //        .Include(x => x.Project)
            //        .ToList();
            //}

            //using (var db = new EmployeeContext())
            //{
            //    var es = db.Employees
            //        .Include(x => x.Company)
            //        .ToList();
            //}

            //using (var db = new ProjectContext())
            //{
            //    var ps = db.Projects
            //        .Where(x => x.Id == 3045)
            //        .Include(x => x.Account)
            //        .Include(x => x.Account)
            //        .Include(x => x.CalendarPlan)
            //        .Include(x => x.Company)
            //        .Include(x => x.ProjectAccess)
            //        .Include(x => x.ProjectPathToFile)
            //        .Include(x => x.Settings)
            //        .Include(x => x.Task)
            //        .Include(x => x.Status)
            //        .Include(x => x.ProjectType)
            //        .ToList();
            //}

            //using (var db = new ProjectAccessContext())
            //{
            //    var pas = db.ProjectAccesses
            //        .Include(x => x.Account)
            //        .Include(x => x.Project)
            //        .ToList();
            //}

            //using (var db = new ProjectPathToFileContext())
            //{
            //    var pptf = db.ProjectPathToFiles
            //        .Include(x => x.Project)
            //        .ToList();
            //}

            //using (var db = new ProjectTypeContext())
            //{
            //    var ptss = db.ProjectTypes
            //        .Include(x => x.Project)
            //        .Include(x => x.ProjectTypeAccess)
            //        .ToList();
            //}

            //using (var db = new ProjectTypeAccessContext())
            //{
            //    var pta = db.ProjectTypeAccesses
            //        .Include(x => x.Account)
            //        .Include(x => x.ProjectType)
            //        .ToList();
            //}

            //using (var db = new StatusContext())
            //{
            //    var s = db.Statuses
            //        .Include(x => x.Project)
            //        .Include(x => x.Task)
            //        .ToList();
            //}

            //using (var db = new TimeTrackContext())
            //{
            //    var tt = db.TimeTracks
            //        .Include(x => x.Account)
            //        .ToList();
            //}
        }
    }
}