using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.CustomAuthorizeAttribute
{
    public class ControllerAccessAttribute : AuthorizeAttribute
    {
        private static string Action { get; set; }
        private static string Controller { get; set; }
        private static string Login { get; set; }
        private static AccountAccess AccountAccess { get; set; }
        private static HttpContextBase HttpContext { get; set; }
        private static RouteData RouteData { get; set; }
        private static bool fromController = false;
        private static string role { get; set; }

        //Dictionary< "Controller" , "Dictionary< 'Action', 'AccessField'>">
        public static readonly Dictionary<string, Dictionary<string, GetAccess>> AccessMethod = new Dictionary<string, Dictionary<string, GetAccess>>
        {
            {
                "Account",
                new Dictionary<string, GetAccess>()
                {
                    {"Create", AccountCreateAccess},
                    {"Delete", AccountCreateAccess},
                    {"Update", AccountCreateAccess},
                    {"PasswordField", AccountPasswordFieldAccess},
                }
            },
            {
                "AccountAccess",
                new Dictionary<string, GetAccess>()
                {
                    {"Index", AccountAccessIndex},
                    {"ProjectAccess", AccountAccessProjectAccess},
                }
            },
            {
                "Calendar",
                new Dictionary<string, GetAccess>()
                {
                    { "EventChangeDateStart", CalendarEdit},
                    { "CreateEvent", CalendarEdit},
                    { "DeleteEvent", CalendarEdit}
                }
            },
            {
                "Company",
                new Dictionary<string, GetAccess>()
                {
                    { "Delete", CompanyCreateEditDelete},
                    { "Edit", CompanyCreateEditDelete},
                    { "Create", CompanyCreateEditDelete},
                }
            },
            {
                "Employee",
                new Dictionary<string, GetAccess>()
                {
                    { "Edit", EmployeeEditDelete},
                    { "Delete", EmployeeEditDelete},
                }
            },
            {
                "Home",
                new Dictionary<string, GetAccess>()
                {
                    { "PatchNotes", AdminPage},
                    { "ClearCache", AdminPage}
                }
            },
            {
                "Project",
                new Dictionary<string, GetAccess>()
                {
                    { "Create", ProjectCreate},
                    { "Edit", ProjectEdit},
                    { "Delete" , ProjectDelete},
                    { "Restore",  ProjectRestore},
                    { "CalendarPlanEdit" , ProjectCalendarPlanEdit },
                    { "CostField" , ProjectCostField },
                    { "StoryPointField" , StoryPointField },
                    { "ProjectResponsible", ProjectResponsible},
                    { "ChengeHistoryProject", ProjectChengeHistory}
                }
            },
            {
                "ProjectType",
                new Dictionary<string, GetAccess>()
                {
                    {"Exel" , ProjectTypeExcel},
                }
            },
            {
                "Settings",
                new Dictionary<string, GetAccess>()
                {
                    { "Index", SettingsAccess},
                    { "UpdateSysAdmin", SettingsAccess},
                    { "UpdateProjectForSysAdmin", SettingsAccess},
                }
            },
            {
                "Task",
                new Dictionary<string, GetAccess>()
                {
                    { "Edit", TaskResponsible},
                    { "Delete", TaskResponsible},
                    { "Create", TaskCreate},
                    { "Restore", TaskRestore},
                    { "ChengeHistoryTask", TaskChengeHistoryTask},
                    { "IsPrivateField", TaskIsPrivateField},
                }
            },
        };

        private static bool ProjectChengeHistory()
        {
            return AccountAccess.ProjectChangeHistory;
        }

        private void ParseContext(HttpContextBase httpContext)
        {
            HttpContext = httpContext;
            RouteData = httpContext.Request.RequestContext.RouteData;
            Action = RouteData.GetRequiredString("action");
            Controller = RouteData.GetRequiredString("controller");
            Login = httpContext.User.Identity.Name;
            AccountAccess = Logic.Logic.Instance.AccountAccess.GetAccessForAccount(Login);
            role = Logic.Logic.Instance.Account.GetRole(Login);
        }

        private static int ProjectTypeId { get; set; }
        private static int ProjectId { get; set; }
        private static int TaskId { get; set; }

        public static void FillFromPage(int projectTypeId = 0, int projectId = 0, int taskId = 0)
        {
            ProjectTypeId = projectTypeId;
            ProjectId = projectId;
            TaskId = taskId;

            Login = ModelHelper.CurrentUserLogin;
            AccountAccess = Logic.Logic.Instance.AccountAccess.GetAccessForAccount(Login);
            role = Logic.Logic.Instance.Account.GetRole(Login);
            fromController = false;
        }

        public delegate bool GetAccess();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (string.IsNullOrEmpty(Login))
            {
                AccountAccess = new AccountAccess("")
                {
                    ProjectAccesses = new List<ProjectAccess>(),
                    ProjectTypeAccesses = new List<ProjectTypeAccess>()
                };
            }

            if (AccountAccess.AccountAccessPage)
            {
                return true;
            }

            ParseContext(httpContext);
            fromController = true;

            return AccessMethod[Controller][Action]();
        }

        public static bool GetAccessPage(string controller, string action)
        {
            if (string.IsNullOrEmpty(Login))
            {
                AccountAccess = new AccountAccess("")
                {
                    ProjectAccesses = new List<ProjectAccess>(),
                    ProjectTypeAccesses = new List<ProjectTypeAccess>()
                };
            }

            if (AccountAccess.AccountAccessPage)
            {
                return true;
            }

            return AccessMethod[controller][action]();

        }

        #region AccountController
        public static bool AccountCreateAccess()
        {
            return AccountAccess.AccountCreate;
        }
        #endregion

        #region AccountAccessController
        public static bool AccountAccessIndex()
        {
            return AccountAccess.AccountAccessPage;
        }
        public static bool AccountAccessProjectAccess()
        {
            var pId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out pId))
                {
                    return true;
                }
            }
            else
            {
                pId = ProjectId;
            }

            var project = Logic.Logic.Instance.Project.Get(pId);
            var pAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);
                        
            return pAccess != null && pAccess.ProjectAccessPage;

        }

        public static bool AccountPasswordFieldAccess()
        {
            return AccountAccess.AccountPasswordField;
        }
        

        #endregion

        #region CalendarController
        public static bool CalendarEdit()
        {
            return AccountAccess.CalendarEdit;
        }
        #endregion

        #region CompanyController
        public static bool CompanyCreateEditDelete()
        {
            return AccountAccess.CompanyEditDelete;
        }
        #endregion

        #region EmployeeController
        public static bool EmployeeEditDelete()
        {
            return AccountAccess.CompanyEmployeeEditDelete;
        }
        #endregion

        #region HomeController
        public static bool AdminPage()
        {
            return AccountAccess.AdminPage;
        }
        #endregion

        #region Project
        public static bool ProjectCreate()
        {
            var ptId = 0;

            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["projectTypeId"] is null ? "" : RouteData.Values["projectTypeId"].ToString(), out ptId))
                {
                    return true;
                }
            }
            else
            {
                ptId = ProjectTypeId;
            }

            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == ptId);
            if (ptAccess is null)
            {
                return false;
            }
            else
            {
                return ptAccess.CreateNew;
            }
        }
        public static bool ProjectEdit()
        {
            var pId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out pId))
                {
                    return true;
                }
            }
            else
            {
                pId = ProjectId;
            }

            return ProjectEdit(pId);
        }


        public static bool ProjectEdit(int pId)
        {
            var project = Logic.Logic.Instance.Project.Get(pId);
            var pAccess = AccountAccess.ProjectAccesses.Find(item=> item.ProjectId == project.Id);
            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);

            return (pAccess != null && pAccess.Responsible) || (ptAccess != null && ptAccess.Edit);
        }

        public static bool ProjectDelete()
        {
            var pId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out pId))
                {
                    return true;
                }
            }
            else
            {
                pId = ProjectId;
            }

            return ProjectDelete(pId);
        }
        public static bool ProjectDelete(int pId)
        {
            var project = Logic.Logic.Instance.Project.Get(pId);
            var pAccess = AccountAccess.ProjectAccesses.Find(item => item.ProjectId == project.Id);
            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);

            return (pAccess != null && pAccess.Responsible) || (ptAccess != null && ptAccess.Delete);
        }

        public static bool ProjectRestore()
        {
            var pId = 0;
            var ptId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out pId))
                {
                    return true;
                }
            }
            else
            {
                pId = ProjectId;
                ptId = ProjectTypeId;
            }


            if (ptId == 0)
            {
                ptId = Logic.Logic.Instance.Project.Get(pId).ProjectType.Id;
            }
            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == ptId);
            if (ptAccess is null)
            {
                return false;
            }
            else
            {
                return ptAccess.DeletedProjectAccess;
            }

        }
        private static bool ProjectCostField()
        {
            if (ProjectTypeId == 0 && RouteData != null)
            {
                if (!int.TryParse(RouteData.Values["projectTypeId"] is null ? "" : RouteData.Values["projectTypeId"].ToString(), out var ptId))
                {
                    ProjectTypeId = ptId;
                }
            }


            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == ProjectTypeId);
            if (ptAccess is null)
            {
                return false;
            }
            else
            {
                return ptAccess.CostFieldProject;
            }
        }

        private static bool ProjectResponsible()
        {
            var pAccess = AccountAccess.ProjectAccesses.Find(item => item.ProjectId == ProjectId);
            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == ProjectTypeId);

            return (ptAccess != null && (ptAccess.Responsible || ptAccess.Delete)) || (pAccess != null && (pAccess.Responsible || pAccess.Delete));
        }

        private static bool StoryPointField()
        {
            var pAccess = AccountAccess.ProjectAccesses.Find(item => item.ProjectId == ProjectId);
            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == ProjectTypeId);

            return (pAccess != null && pAccess.StoryPointsFieldTask) || (ptAccess != null && ptAccess.StoryPointsFieldTask);
        }

        #endregion

        #region ProejctTypeController
        public static bool ProjectTypeExcel()
        {
            var ptId = 0;

            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["projectTypeId"] is null ? "" : RouteData.Values["projectTypeId"].ToString(), out ptId))
                {
                    return true;
                }
            }
            else
            {
                ptId = ProjectTypeId;
            }

            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == ptId);
            if (ptAccess is null)
            {
                return false;
            }
            else
            {
                return ptAccess.ExelReport;
            }
        }

        private static bool ProjectCalendarPlanEdit()
        {
            var pId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out pId))
                {
                    return true;
                }
            }
            else
            {
                pId = ProjectId;
            }

            var pAccess = AccountAccess.ProjectAccesses.Find(item => item.ProjectId == pId);
            if (pAccess is null)
            {
                return false;
            }
            else
            {
                return pAccess.CalendarPlanEdit;
            }
        }
        #endregion

        #region SettingsController
        public static bool SettingsAccess()
        {
            return AccountAccess.AccountAccessPage;
        }
        #endregion

        #region TaskController
        public static bool TaskResponsible()
        {
            var tId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out tId))
                {
                    return true;
                }
            }
            else
            {
                tId = TaskId;
            }


            var task = Logic.Logic.Instance.Task.Get(tId);
            var project = Logic.Logic.Instance.Project.Get(task.ProjectId);

            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);
            var pAccess = AccountAccess.ProjectAccesses.Find(item => item.ProjectId == project.Id);

            return ModelHelper.IsTaskResponsible(task.Account.Login, pAccess, ptAccess);
        }

        public static bool TaskRestore()
        {
            var tId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["id"] is null ? "" : RouteData.Values["id"].ToString(), out tId))
                {
                    return true;
                }
            }
            else
            {
                tId = TaskId;
            }


            var task = Logic.Logic.Instance.Task.Get(tId);
            var project = Logic.Logic.Instance.Project.Get(task.ProjectId);

            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);


            return ptAccess is null ? false : ptAccess.RestoreTask;
        }

        public static bool TaskCreate()
        {
            var pId = 0;
            if (fromController)
            {
                if (!int.TryParse(RouteData.Values["projectId"] is null ? "" : RouteData.Values["projectId"].ToString(), out pId))
                {
                    return true;
                }
            }
            else
            {
                pId = ProjectId;
            }


            var project = Logic.Logic.Instance.Project.Get(pId);

            var pAccess = AccountAccess.ProjectAccesses.Find(item => item.ProjectId == project.Id);
            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);

            return (pAccess != null && (pAccess.CreateNew || pAccess.Responsible)) || (ptAccess != null && ptAccess.Responsible);
        }

        public static bool TaskChengeHistoryTask()
        {
            var project = Logic.Logic.Instance.Project.Get(ProjectId);

            var ptAccess = AccountAccess.ProjectTypeAccesses.Find(item => item.ProjectTypeId == project.ProjectType.Id);

            return ptAccess != null && ptAccess.ChengeHistoryTask;
        }

        public static bool TaskIsPrivateField()
        {
            if (TaskId != 0)
            {
                var task =  Logic.TaskBLL.Instance.Get(TaskId);
                if (task != null)
                {
                    if (task.Reporter?.Login == Login && task.Account.Login == Login)
                    {
                        return true;
                    }
                }
            }
            return Role.AdminBuh.Contains(role);
        }
        #endregion
    }
}