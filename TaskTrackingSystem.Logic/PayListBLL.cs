using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TaskTrackingSystem.DAL.EFDAL.Implementation;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic
{
    public class PayListBLL
    {
        #region singleton
        public static PayListBLL Instance { get; private set; }

        private readonly PayListDAL payListDAL;

        private PayListBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    //PayListDAL = new CalendarPlanDAL();
                    break;
                case "Local":
                    //PayListDAL = new CalendarPlanDAL();
                    break;
                case "EFDB":
                    payListDAL = new PayListDAL();
                    break;
            }
        }

        static PayListBLL()
        {
            Instance = new PayListBLL();
        }
        #endregion

        public void Add(List<PayList> payLists)
        {
            foreach (var pl in payLists)
            {
                Add(pl);
            }
        }

        public void Add(PayList pl)
        {
            if (pl.Login != null)
            {
                var founddPL = payListDAL.GetForAccount(pl.Login, pl.Date);
                if (founddPL is null)
                {
                    if (payListDAL.Create(pl) != 0)
                    {
                        pl.StatusBD = "Добавлено в БД";
                    }
                }
                else
                {
                    pl.Id = founddPL.Id;
                    if (payListDAL.Update(pl))
                    {
                        pl.StatusBD = "Обновлено в БД";
                    }
                }
            }
            else
            {
                pl.StatusBD = "Логина нет";
            }
        }

        public List<PayList> GetForAccount(string login)
        {
            return payListDAL.GetForAccount(login);
        }

        public List<PayList> ParcePayList(string path)
        {
            var pll = Files.Exel.ParcePayList(path);

            var accs = Logic.Instance.Account.GetAll(true).OrderBy(x => x.Name);

            foreach (var a in accs)
            {
                Debug.WriteLine($"acc {a.Name}");
            }

            foreach (var pl in pll)
            {
                MapPayListItem(pl);
            }

            return pll;
        }

        private void MapPayListItem(PayList pl)
        {
            var accs = Logic.Instance.Account.GetAll(true);

            // Поиск Логина по имени
            var fio = pl.Name.Split(' ');

            Account account = null;

            var accByF = accs.Where(x => x.Name.Contains(fio[0]));

            if (accByF.Count() == 1)
            {
                account = accByF.FirstOrDefault();
            }
            else if (accByF.Count() > 1)
            {
                accByF.Where(x => x.Name.Contains(fio[1]));
                account = accByF.FirstOrDefault();
            }
            else if (accByF.Count() == 0)
            {
            }

            if (account is null)
            {
                Debug.WriteLine($"Not {pl.Name}");
                pl.Error += $" Сотрудник не найден : {pl.Name}";
            }
            else
            {
                Debug.WriteLine($"Yes {account.Login}");
                pl.Login = account.Login;
            }

            // Поиск даты листа
            var monthYearString = pl.Title.Replace("РАСЧЕТНЫЙ ЛИСТОК ЗА ", "");

            var my = monthYearString.Split(' ');

            pl.Date = new DateTime(int.Parse(my[1]), GetMonthNumberFromMonthName(my[0]) , 1);
        }

        public PayList GetForAccount(string login, int year, int month)
        {
            return payListDAL.GetForAccount(login, new DateTime(year, month, 1));            
        }

        private static int GetMonthNumberFromMonthName(string monthname)
        {
            var monthNumber = 0;
            monthNumber = DateTime.ParseExact(monthname, "MMMM", CultureInfo.CurrentCulture).Month;
            return monthNumber;
        }
    }
}
