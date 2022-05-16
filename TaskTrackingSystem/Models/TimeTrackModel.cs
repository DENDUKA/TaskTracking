using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Models
{
    public class TimeTrackModel : IValidatableObject
    {
        public int Id { get; set; }

        public Account Account { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DisplayName("Сотрудник")]
        [DataType(DataType.Text)]
        public string AccountLogin { get; set; }

        [DisplayName("Время")]
        [DataType(DataType.Text)]
        public string TimeString { get; set; }

        [Required]
        [DisplayName("Тип")]
        public string Type { get; set; }

        [DisplayName("С")]
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime DateStartTime { get; set; }

        [DisplayName("По")]
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateEnd { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime DateEndTime { get; set; }

        [Required]
        [DisplayName("Время")]
        public int Time { get; set; }

        [DisplayName("Описание")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public static readonly Regex RegexTimeString = new Regex(@"(?:(\d+)h ?)?(?:(\d+)m)?");

        public IEnumerable<SelectListItem> DropDownListTypes => ModelHelper.GetDropDownListTypes();

        public IEnumerable<SelectListItem> DropDownListAccounts => ModelHelper.GetDropDownListAccounts();


        public static List<TimeTrackModel> GetAll()
        {
            var model = new List<TimeTrackModel>();
            foreach (var x in Logic.Logic.Instance.TimeTrack.GetAll())
            {
                model.Add((TimeTrackModel)x);
            }
            return model;
        }

        public static int TimeSum(List<TimeTrackModel> listModel)
        {
            var result = 0;

            result += listModel.Where(x => TimeTrack.ActionPlusTime.Contains(x.Type)).Sum(x => x.Time);
            result -= listModel.Where(x => TimeTrack.ActionMinusTime.Contains(x.Type)).Sum(x => x.Time);

            return result;
        }
        public static int TimeFromString(string timeString)
        {
            var time = 0;

            var match = RegexTimeString.Match(timeString);

            if (int.TryParse(match.Groups[1].Value, out var hours))
            {
                time += hours * 60;
            }

            if (int.TryParse(match.Groups[2].Value, out var mins))
            {
                time += mins;
            }

            return time;
        }

        public static string TimeToString(int time)
        {
            if (time == 0)
            {
                return "0h";
            }

            var h = time / 60;
            var m = time - (h * 60);

            return $"{(time < 0 ? "-" : "")}{(h == 0 ? "" : $"{Math.Abs(h)}h")} {(m == 0 ? "" : $"{Math.Abs(m)}m")}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (DateEnd <= DateStart)
            {
                if (DateEnd == DateStart)
                {
                    if (DateEndTime <= DateStartTime)
                    {
                        results.Add(new ValidationResult("Время окончания должно быть больше времени начала", new[] { "DateEnd" }));
                    }
                }
                else
                {
                    results.Add(new ValidationResult("Дата окончания должна быть больше даты начала", new[] { "DateEnd" }));
                }
            }

            if (DateStart < DateTime.Today.Date.AddDays(-7))
            {
                results.Add(new ValidationResult("Дата должна быть не раньше чем неделю назад", new[] { "DateStart" }));
            }

            return results;
        }

        public static int GetTime(DateTime start, DateTime end)
        {
            DateTime startWorkDay;
            DateTime endWorkDay;
            DateTime startBreak;
            DateTime endBreak;

            var current = start;
            var time = 0;

            while (current <= end)
            {
                startWorkDay = new DateTime(current.Year, current.Month, current.Day, 9, 0, 0);
                startBreak = new DateTime(current.Year, current.Month, current.Day, 13, 0, 0);
                endBreak = new DateTime(current.Year, current.Month, current.Day, 14, 0, 0);
                endWorkDay = new DateTime(current.Year, current.Month, current.Day, 18, 0, 0);

                if (current < endWorkDay)
                {
                    if (current < startWorkDay)
                    {
                        current = startWorkDay;
                        if (current >= end)
                        {
                            break;
                        }
                    }

                    if (end <= startBreak)
                    {
                        time += (int)(end - current).TotalMinutes;
                        break;
                    }
                    else
                    {
                        if (current >= end)
                        {
                            break;
                        }
                        if (current < startBreak)
                        {
                            time += (int)(startBreak - current).TotalMinutes;
                            current = endBreak;
                        }
                    }

                    if (end <= endWorkDay)
                    {
                        time += (int)(end - current).TotalMinutes;
                        break;
                    }
                    else
                    {
                        time += (int)(endWorkDay - current).TotalMinutes;
                        current = endWorkDay;
                    }
                }

                current = current.AddHours(15);
            }

            return time;
        }

        public static explicit operator TimeTrackModel(TimeTrack timeTrack)
        {
            var model = new TimeTrackModel()
            {
                Id = timeTrack.Id,
                Account = timeTrack.Account,
                DateStart = timeTrack.DateStart,
                DateEnd = timeTrack.DateEnd,
                Type = timeTrack.Type,
                Description = timeTrack.Description,
                AccountLogin = timeTrack.Login,
            };

            if (TimeTrack.ActionPlusTime.Contains(model.Type))
            {
                model.Time = GetTime(model.DateStart, model.DateEnd);
            }
            else if (TimeTrack.ActionMinusTime.Contains(model.Type))
            {
                model.Time = (int)(model.DateEnd - model.DateStart).TotalMinutes;
            }


            model.TimeString = TimeToString(model.Time);

            return model;
        }

        public static explicit operator TimeTrack(TimeTrackModel model)
        {
            var tt = new TimeTrack()
            {
                Id = model.Id,
                Account = model.Account,
                Type = model.Type,
                DateEnd = model.DateEnd,
                DateStart = model.DateStart,
                Description = model.Description,
                Login = model.AccountLogin,
            };

            return tt;
        }
    }
}