using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.Models
{
    public static class ModelHelper
    {
        public static string GetDateFormatForSorttable(DateTime dt)
        {
            return dt.ToString("yyyMMddHHmmss");
        }

        public static IEnumerable<SelectListItem> GetDropDownListContractStatuses(string selected = "Нет")
        {
            if (string.IsNullOrEmpty(selected))
            {
                selected = "Нет";
            }

            var listItem = new List<SelectListItem>();

            foreach (var x in Project.ContractStatusList)
            {
                listItem.Add(new SelectListItem
                {
                    Text = x,
                    Value = x,
                    Selected = x == selected,
                });
            }

            return listItem;
        }

        public static IEnumerable<SelectListItem> GetDropDownListStatuses()
        {
            var listItem = new List<SelectListItem>();

            foreach (var x in Logic.Logic.Instance.Status.GetAll())
            {
                listItem.Add(new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
            }

            listItem.RemoveAll(x => x.Text.Equals(Status.Deleted));

            var InWorkItem = listItem.Find(x => x.Text.Equals(Status.InWork));
            listItem.RemoveAll(x => x.Text.Equals(Status.InWork));
            listItem.Insert(0, InWorkItem);

            return listItem;
        }

        internal static IEnumerable<SelectListItem> GetDropDownListTypes()
        {
            var listItem = new List<SelectListItem>();

            foreach (var x in TimeTrack.ActionPlusTime)
            {
                listItem.Add(new SelectListItem() { Text = x, Value = x });
            }

            foreach (var x in TimeTrack.ActionMinusTime)
            {
                listItem.Add(new SelectListItem() { Text = x, Value = x });
            }

            listItem.OrderBy(x => x.Text);

            return listItem;
        }

        public static IEnumerable<SelectListItem> GetDropDownListProjectForProjectType(int projectTypeId, int? selectedId = null)
        {
            var projects = Logic.Logic.Instance.Project.GetAllForProjectType(projectTypeId);

            var listItem = new List<SelectListItem>();

            foreach (var x in projects)
            {
                listItem.Add(new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
            }

            if (selectedId != null)
            {
                var item = listItem.Find(x => x.Value == selectedId.ToString());
                item.Selected = true;
            }

            return listItem;
        }

        public static IEnumerable<SelectListItem> GetDropDownListProjectType(int? selectedId = null)
        {
            var listItem = new List<SelectListItem>();

            foreach (var pt in Logic.Logic.Instance.ProjectType.GetAll())
            {
                listItem.Add(new SelectListItem
                {
                    Text = pt.Name,
                    Value = pt.Id.ToString(),
                });
            }

            if (selectedId is null)
            {
                listItem.Insert(0, new SelectListItem { Text = "-------", Value = "0" });
            }
            else
            {
                var item = listItem.Find(x => x.Value == selectedId.ToString());
                item.Selected = true;
            }

            return listItem;
        }

        public static IEnumerable<SelectListItem> GetDropDownListAccounts(string loginSelected = null, bool isNeedEmpty = true)
        {

            var listItem = new List<SelectListItem>();

            if (isNeedEmpty)
            {
                listItem.Add(new SelectListItem { Text = "", Value = "", });
            }

            foreach (var x in Logic.Logic.Instance.Account.GetAll())
            {
                listItem.Add(new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Login,
                });
            }

            if (loginSelected != null)
            {
                var item = listItem.Find(x => x.Value.Equals(loginSelected));

                if (item != null)
                {
                    item.Selected = true;
                }
            }

            listItem = listItem.OrderBy(item => item.Text).ToList();

            return listItem;
        }

        public static IEnumerable<SelectListItem> GetDropDownListRoles()
        {
            var listItem = new List<SelectListItem>();

            foreach (var x in Role.RoleList.OrderByDescending(x => x))
            {
                listItem.Add(new SelectListItem() { Value = x, Text = x });
            }

            return listItem;
        }

        public static bool IsCurrentUserAdmin()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return CurrentRole.Equals(Role.Admin);
            }
            return false;
        }

        public static bool IsCurrentUserAdminBuh()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Role.AdminBuh.Contains(CurrentRole);
            }
            return false;
        }

        public static string CurrentUserLogin => HttpContext.Current.User.Identity.Name;

        public static string CurrentRole => Logic.Logic.Instance.Account.GetRole(CurrentUserLogin);

        public static DateTime MinSQLDate = new DateTime(1800, 1, 1);

        internal static IEnumerable<SelectListItem> GetDropDownListPockerMarks()
        {
            var marksArray = new string[] { "1", "2", "3", "5", "8", "13", "20", "40", "100" };

            var listItem = new List<SelectListItem>
            {
                new SelectListItem() { Value = "0", Text = "0 (без оценки)" }
            };

            foreach (var x in marksArray)
            {
                listItem.Add(new SelectListItem() { Value = x, Text = x });
            }

            return listItem;
        }

        public static bool GetBoolValue(object obj, string fieldName)
        {
            return (bool)obj.GetType().GetProperty(fieldName).GetValue(obj);
        }

        private static readonly Dictionary<int, byte> StatusSortableKey = new Dictionary<int, byte>
        {
            { Status.InWorkId, 1 },
            { Status.PostpondedId, 2 },
            { Status.DoneId, 3 },
            { Status.DeletedId, 4 }
        };

        public static byte GetStatusKeyForSorttable(int id)
        {
            return StatusSortableKey[id];
        }

        public static string FormatDateTime(DateTime time)
        {
            return time.ToString("dd.MM.yyyy HH:mm");
        }

        #region Access
        public static bool IsTaskResponsible(string taskResponsibleLogin, ProjectAccess pAccess, ProjectTypeAccess ptAccess)
        {
            return
                 taskResponsibleLogin.Equals(CurrentUserLogin) ||
                 (pAccess != null && pAccess.Responsible) ||
                 (ptAccess != null && ptAccess.Responsible);
        }
        #endregion
    }
}