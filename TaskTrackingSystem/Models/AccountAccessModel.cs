using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Attributes;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.UI.Web.Models
{
    public class AccountAccessModel
    {
        public AccountAccessModel()
        {

        }

        [DisplayName("Логин")]
        [Required]
        public string Login { get; set; }
        [DisplayName("Страница Админа (View)")]
        [AccessFieldMetaData("aaadminpage", 0)]
        public bool AdminPage { get; private set; }
        [DisplayName("Список пользователей (View)")]
        [AccessFieldMetaData("aauserlistpage", 1)]
        public bool UserListPage { get; private set; }
        [DisplayName("Страница редактирования доступа")]
        [AccessFieldMetaData("aaccesspage", 1.1)]
        public bool AccountAccessPage { get; private set; }        
        [DisplayName("Календарь (Edit)")]
        [AccessFieldMetaData("aacalendarpage", 2)]
        public bool CalendarEdit { get; private set; }
        [DisplayName("Список компаний (View)")]
        [AccessFieldMetaData("aacompanyspage", 3)]
        public bool CompanysPage { get; private set; }
        [DisplayName("Создать аккаунт")]
        [AccessFieldMetaData("aaaccountcreate", 7)]
        public bool AccountCreate { get; private set; }
        [DisplayName("Аккаунт -> Пароль (View)")]
        [AccessFieldMetaData("aaaccountpasswordfield", 8)]
        public bool AccountPasswordField { get; private set; }
        [DisplayName("Компания->Сотрудники (Edit Delete)")]
        [AccessFieldMetaData("aacompanyemployeeeditdelete", 9)]
        public bool CompanyEmployeeEditDelete { get; private set; }
        [DisplayName("Компания (Edit Delete)")]
        [AccessFieldMetaData("aacompanyeditdelete", 9)]
        public bool CompanyEditDelete { get; private set; }
        [DisplayName("История изменений (Project)")]
        [AccessFieldMetaData("aaprojectchangehistory", 10)]
        public bool ProjectChangeHistory { get; private set; }

        //Добавить новое поле : 1) добавить в AccountAccess поле с таким же названием 2) в explicit AccountAccessModel добавить новое поле

        public List<ProjectTypeAccess> ProjectTypeAccesses { get; set; }
        public List<ProjectAccess> ProjectAccesses { get; set; }


        public static List<AccountAccessModel> GetAll()
        {
            var model = new List<AccountAccessModel>();

            foreach (var aa in Logic.Logic.Instance.AccountAccess.GetFullAccessForAllAccounts())
            {
                model.Add((AccountAccessModel)aa);
            }

            return model;
        }

        public IEnumerable<SelectListItem> DropDownListProjectType(int? selectedPTId) => ModelHelper.GetDropDownListProjectType(selectedPTId);

        public static explicit operator AccountAccessModel(AccountAccess aa)
        {
            return new AccountAccessModel
            {
                Login = aa.Login,
                AdminPage = aa.AdminPage,
                UserListPage = aa.UserListPage,
                CalendarEdit = aa.CalendarEdit,
                CompanysPage = aa.CompanysPage,
                ProjectTypeAccesses = aa.ProjectTypeAccesses,
                ProjectAccesses = aa.ProjectAccesses,
                AccountCreate = aa.AccountCreate,
                AccountPasswordField = aa.AccountPasswordField,
                AccountAccessPage = aa.AccountAccessPage,
                CompanyEmployeeEditDelete = aa.CompanyEmployeeEditDelete,
                CompanyEditDelete = aa.CompanyEditDelete,
                ProjectChangeHistory = aa.ProjectChangeHistory,
            };
        }

        #region AccountAccessMeta
        public static List<AccessFieldMetaData> MetaDataAA
        {
            get
            {
                if (_metaDataAccount is null)
                {
                    _metaDataAccount = GetAAMetaData();
                }
                return _metaDataAccount;
            }
        }

        private static List<AccessFieldMetaData> _metaDataAccount = null;

        private static List<AccessFieldMetaData> GetAAMetaData()
        {
            var typeMetaData = typeof(AccessFieldMetaData);
            var typeDysplayName = typeof(DisplayNameAttribute);

            var options = typeof(AccountAccessModel).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(n => Attribute.IsDefined(n, typeMetaData) && Attribute.IsDefined(n, typeDysplayName));

            var myAttrList = new List<AccessFieldMetaData>();

            foreach (var info in options)
            {
                var x = info.GetCustomAttributes(typeMetaData, true);

                var metaData = (AccessFieldMetaData)info.GetCustomAttributes(typeMetaData, true)[0];
                var displayName = (DisplayNameAttribute)info.GetCustomAttributes(typeDysplayName, true)[0];

                metaData.DispalayName = displayName.DisplayName;
                metaData.FieldName = info.Name;
                myAttrList.Add(metaData);
            }

            return myAttrList.OrderBy(x => x.Order).ToList();
        }
        #endregion

        #region ProjectTypeAccessMeta

        public static List<AccessFieldMetaData> MetaDataPTA
        {
            get
            {
                if (_metaDataPTA is null)
                {
                    _metaDataPTA = GetPTAMetaData();
                }
                return _metaDataPTA;
            }
        }

        private static List<AccessFieldMetaData> _metaDataPTA = null;

        private static List<AccessFieldMetaData> GetPTAMetaData()
        {
            var typeMetaData = typeof(AccessFieldMetaData);
            var typeDysplayName = typeof(DisplayNameAttribute);

            var options = typeof(ProjectTypeAccess).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(n => Attribute.IsDefined(n, typeMetaData) && Attribute.IsDefined(n, typeDysplayName));

            var myAttrList = new List<AccessFieldMetaData>();

            foreach (var info in options)
            {
                var x = info.GetCustomAttributes(typeMetaData, true);

                var metaData = (AccessFieldMetaData)info.GetCustomAttributes(typeMetaData, true)[0];
                var displayName = (DisplayNameAttribute)info.GetCustomAttributes(typeDysplayName, true)[0];

                metaData.DispalayName = displayName.DisplayName;
                metaData.FieldName = info.Name;
                myAttrList.Add(metaData);
            }

            return myAttrList.OrderBy(x => x.Order).ToList();
        }
        #endregion

        #region ProjectAccessMeta
        public static List<AccessFieldMetaData> MetaDataPA
        {
            get
            {
                if (_metaDataPA is null)
                {
                    _metaDataPA = GetPAMetaData();
                }
                return _metaDataPA;
            }
        }

        private static List<AccessFieldMetaData> _metaDataPA = null;

        private static List<AccessFieldMetaData> GetPAMetaData()
        {
            var typeMetaData = typeof(AccessFieldMetaData);
            var typeDysplayName = typeof(DisplayNameAttribute);

            var options = typeof(ProjectAccess).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(n => Attribute.IsDefined(n, typeMetaData) && Attribute.IsDefined(n, typeDysplayName));

            var myAttrList = new List<AccessFieldMetaData>();

            foreach (var info in options)
            {
                var x = info.GetCustomAttributes(typeMetaData, true);

                var metaData = (AccessFieldMetaData)info.GetCustomAttributes(typeMetaData, true)[0];
                var displayName = (DisplayNameAttribute)info.GetCustomAttributes(typeDysplayName, true)[0];

                metaData.DispalayName = displayName.DisplayName;
                metaData.FieldName = info.Name;
                myAttrList.Add(metaData);
            }

            return myAttrList.OrderBy(x => x.Order).ToList();
        }
        #endregion
    }
}