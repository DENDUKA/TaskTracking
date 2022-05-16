using System.Collections.Generic;

namespace TaskTrackingSystem.Shared.Entities
{
    public static class Role
    {
        public const string Admin = "admin";
        public const string User = "user";
        public const string Buh = "buh";
        public static string Dev = "dev";
        public const string Lead1 = "lead1";//Геолого-разведочные работы
        public const string Lead2 = "lead2";//Сейсморазведка
        public const string Lead3 = "lead3";//Подсчет запасов
        public const string Lead4 = "lead4";//Разработка
        public const string Lead5 = "lead5";//Другие проекты
        public const string Leads = ", lead1,lead2,lead3,lead4,lead5";
        public const string AdminLead = "admin,buh" + Leads;
        public const string AdminBuh = "admin,buh";
        public const string AdminLeadUser = "admin,buh" + Leads + ",user";

        //TODO удалить
        public static string[] AdmiBuhList = new string[] { Admin, Buh };
        public static string[] AdminLeadList = new string[] { Admin, Buh, Lead1, Lead2, Lead3, Lead4, Lead5 };
        //_____

        public static string[] RoleList = new string[] { Admin, Buh, Dev, User };
        public static string[] AdminAccess = new string[] { Admin, Buh, Lead1, Lead2, Lead3, Lead4, Lead5, User };
        public static string[] BuhAccess = new string[] { Buh, Lead1, Lead2, Lead3, Lead4, Lead5, User };
        public static string[] LeadAccess = new string[] { Lead1, Lead2, Lead3, Lead4, Lead5, User };
        public static string[] UserAccess = new string[] { Lead1, Lead2, Lead3, Lead4, Lead5, User };

        public static Dictionary<int, string> ProjectTypeLeads = new Dictionary<int, string>
        {
            { 1 , Lead1 },
            { 2 , Lead2 },
            { 3 , Lead3 },
            { 4 , Lead4 },
            { 5 , Lead5 },
        };

        
    }
}