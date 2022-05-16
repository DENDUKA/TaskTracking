using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using TaskTrackingSystem.Logic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class PayListModel
    {
        public List<PayList> PayList { get; set; }

        public void Parce(string path)
        {
            PayList = PayListBLL.Instance.ParcePayList(path);

            PayListBLL.Instance.Add(PayList);
        }

        public static IEnumerable<SelectListItem> YearsToSelectedList()
        {
            var selectedList = new List<SelectListItem>();

            for (var i = DateTime.Now.Year; i >= DateTime.Now.Year - 10; i--)
            {
                selectedList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return selectedList;
        }

        public static IEnumerable<SelectListItem> MonthToSelectedList()
        {
            var selectedList = new List<SelectListItem>();

            for (var i = 1; i <= 12; i++)
            {
                var sli = new SelectListItem()
                {
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                    Value = i.ToString()
                };

                selectedList.Add(sli);

                if (i == DateTime.Now.Month)
                {
                    sli.Selected = true;
                }
            }

            return selectedList;
        }
    }
}

