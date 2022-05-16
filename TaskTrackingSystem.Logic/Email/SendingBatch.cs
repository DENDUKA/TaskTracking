using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Email
{
    public static class SendingEmailBatch
    {
        private static int sendH = 9;
        private static int sendM = 0;
        private static int d = 15;

        private static readonly Timer Every30Minutes = new Timer(1800000);
        private static DateTime timeSend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sendH, sendM, 0);

        public static void Start()
        {
            SendNotifications(null, null);
            if (!Every30Minutes.Enabled)
            {
                Every30Minutes.Elapsed += SendNotifications;
                Every30Minutes.Start();
            }
        }

        public static void Stop()
        {
            Every30Minutes.Stop();
        }

        private static void SendNotifications(object source, ElapsedEventArgs e)
        {
#if DEBUG

            OverdueProjectTime();
#endif


            if (IsSendNotifications())
            {
                EndingTaskTime();
                EndingProjectTime();
                OverdueTaskTime();
                OverdueProjectTime();
            }
        }

        private static void OverdueProjectTime()
        {
            var projects = new List<Project>();

            foreach (var pt in ProjectTypeBLL.Instance.GetMainPTs())
            {
                projects.AddRange(ProjectBLL.Instance.GetAllForProjectType(pt.Id).Where(x => (x.StatusId == Status.InWorkId || x.StatusId == Status.PostpondedId) && x.DateEnd == DateTime.Today.AddDays(-1).Date));
            }

            foreach (var p in projects)
            {
                EmailBLL.OverdueProjectTime(p);
            }
        }

        private static void OverdueTaskTime()
        {
            var tasks = TaskBLL.Instance.GetAll(DateTime.Today.Date.AddDays(-1), DateTime.Today.Date.AddDays(-1), new int[] { Status.InWorkId, Status.PostpondedId }).Where(x => !x.IsPrivate);
            foreach (var t in tasks)
            {
                EmailBLL.OverdueTaskTime(t);
            }
        }

        private static void EndingTaskTime()
        {
            var tasks = TaskBLL.Instance.GetAll(DateTime.Today.Date, DateTime.Today.Date.AddMonths(1), new int[] { Status.InWorkId }).Where(x => !x.IsPrivate);

            foreach (var t in tasks)
            {
                var percentLeft = (t.DateEnd - DateTime.Now.Date).TotalSeconds / (t.DateEnd - t.DateStart).TotalSeconds;
                if (percentLeft <= 0.2D)
                {
                    EmailBLL.TaskEndTime(t);
                }
            }
        }

        private static void EndingProjectTime()
        {
            var projects = ProjectBLL.Instance.GetAllForProjectType(ProjectTypeBLL.Instance.GetMainPTs().Select(x => x.Id).ToArray());

            projects = projects.Where(x => x.DateEnd >= DateTime.Today.Date && x.DateEnd <= DateTime.Today.Date.AddMonths(1) && x.StatusId != Status.DoneId && x.StatusId != Status.DeletedId).ToList();


            foreach (var p in projects)
            {
                var percentLeft = (p.DateEnd - DateTime.Now.Date).TotalSeconds / (p.DateEnd - p.DateStart).TotalSeconds;
                if (percentLeft <= 0.2D)
                {
                    EmailBLL.ProjectEndTime(p);
                }
            }
        }

        private static bool IsSendNotifications()
        {
            return DateTime.Now >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sendH, sendM, 0).AddMinutes(-d) &&
                DateTime.Now <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sendH, sendM, 0).AddMinutes(d);
        }
    }
}
