using System;
using System.Collections.Generic;
using System.Web;
using TaskTrackingSystem.AbstractDAL.Logs;
using TaskTrackingSystem.DAL.ActionLogs;
using TaskTrackingSystem.Logic.Email;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.ActionLogs
{
    public class ActionLogsBLL
    {
        #region singleton
        public static ActionLogsBLL Instance { get; private set; }

        private readonly IActionLogsDAL actionLogDAL;

        private ActionLogsBLL()
        {
            switch (DAL.Shared.DALType)
            {
                case "DB":
                    actionLogDAL = new ActionLogsDAL();
                    break;
                case "Local":
                    actionLogDAL = new TaskLogLocalDAL();
                    break;
                case "EFDB":
                    actionLogDAL = new ActionLogsDAL();
                    break;
            }
        }

        static ActionLogsBLL()
        {
            Instance = new ActionLogsBLL();
        }
        #endregion

        public void LogTaskAction(Task newTask, Task oldTask)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var logType = "Task";
            var changes = new List<string>();

            if (!oldTask.Name.Equals(newTask.Name))
            {
                actionLogDAL.Add(newTask.Id, userName, "Название", oldTask.Name, newTask.Name, logType);
                changes.Add(FieldChangeString("Название", oldTask.Name, newTask.Name));
            }
            if (!oldTask.DateStart.Equals(newTask.DateStart))
            {
                actionLogDAL.Add(newTask.Id, userName, "Дата начала", DateFormat(oldTask.DateStart), DateFormat(newTask.DateStart), logType);
                //changes.Add($"<p><span><strong>Поле \"Дата начала\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldTask.DateStart:dd.MM.yyy}</br>&emsp;&emsp; Новое значение : {newTask.DateStart:dd.MM.yyy}</p>");
            }
            if (!oldTask.DateEnd.Equals(newTask.DateEnd))
            {
                actionLogDAL.Add(newTask.Id, userName, "Дата завершения", DateFormat(oldTask.DateEnd), DateFormat(newTask.DateEnd), logType);
               // changes.Add($"<p><span><strong>Поле \"Дата завершения\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldTask.DateEnd:dd.MM.yyy}</br>&emsp;&emsp; Новое значение : {newTask.DateEnd:dd.MM.yyy}</p>");
            }
            if (!oldTask.Status.Id.Equals(newTask.Status.Id))
            {
                actionLogDAL.Add(newTask.Id, userName, "Статус", oldTask.Status.Id.ToString(), newTask.Status.Id.ToString(), logType);
                EmailBLL.SendTaskClosed(newTask, oldTask);
               // changes.Add($"<p><span><strong>Поле \"Статус\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldTask.Status.Name}</br>&emsp;&emsp; Новое значение : {newTask.Status.Name}</p>");
            }
            if (!oldTask.Account.Login.Equals(newTask.Account.Login))
            {
                actionLogDAL.Add(newTask.Id, userName, "Исполнитель", oldTask.Account.Login, newTask.Account.Login, logType);
                changes.Add(FieldChangeString("Исполнитель", oldTask.Account.Name, newTask.Account.Name));
            }
            if (!oldTask.Description.Equals(newTask.Description))
            {
                actionLogDAL.Add(newTask.Id, userName, "Описание", oldTask.Description, newTask.Description, logType);
                changes.Add(FieldChangeString("Описание", oldTask.Description, newTask.Description));
            }
            if (!oldTask.StoryPoints.Equals(newTask.StoryPoints))
            {
                actionLogDAL.Add(newTask.Id, userName, "Оценка", oldTask.StoryPoints.ToString(), newTask.StoryPoints.ToString(), logType);
               // changes.Add($"<p><span><strong>Поле \"Оценка\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldTask.StoryPoints}</br>&emsp;&emsp; Новое значение : {newTask.StoryPoints}</p>");
            }

            if (changes.Count != 0)
            {
                EmailBLL.TaskChanged(newTask, changes);
            }
        }

        public void LogProjectAction(Project newProject, Project oldProject)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var logType = "Project";
            var changes = new List<string>();

            if (!oldProject.Name.Equals(newProject.Name))
            {
                actionLogDAL.Add(newProject.Id, userName, "Название", oldProject.Name, newProject.Name, logType);
                changes.Add(FieldChangeString("Название", oldProject.Name, newProject.Name));
                //changes.Add($"<p><span><strong>Поле \"Название\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.Name}</br>&emsp;&emsp; Новое значение : {newProject.Name}</p>");
            }
            if (!oldProject.DateStart.Equals(newProject.DateStart))
            {
                actionLogDAL.Add(newProject.Id, userName, "Дата начала", DateFormat(oldProject.DateStart), DateFormat(newProject.DateStart), logType);
                changes.Add(FieldChangeString("Дата начала", oldProject.DateStart.ToString(), newProject.DateStart.ToString()));
                //changes.Add($"<p><span><strong>Поле \"Дата начала\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.DateStart}</br>&emsp;&emsp; Новое значение : {newProject.DateStart}</p>");
            }
            if (!oldProject.DateEnd.Equals(newProject.DateEnd))
            {
                actionLogDAL.Add(newProject.Id, userName, "Дата завершения", DateFormat(oldProject.DateEnd), DateFormat(newProject.DateEnd), logType);
                changes.Add(FieldChangeString("Дата завершения", oldProject.DateEnd.ToString(), newProject.DateEnd.ToString()));
                //changes.Add($"<p><span><strong>Поле \"Дата завершения\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.DateEnd}</br>&emsp;&emsp; Новое значение : {newProject.DateEnd}</p>");
            }
            if (!oldProject.DateEndFact.Equals(newProject.DateEndFact))
            {
                actionLogDAL.Add(newProject.Id, userName, "Фактическая дата завершения", DateFormat(oldProject.DateEndFact), DateFormat(newProject.DateEndFact), logType);
                //changes.Add($"<p><span><strong>Поле \"Фактическая дата завершения\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.DateEndFact}</br>&emsp;&emsp; Новое значение : {newProject.DateEndFact}</p>");
            }
            if (!oldProject.Status.Id.Equals(newProject.Status.Id))
            {
                actionLogDAL.Add(newProject.Id, userName, "Статус", oldProject.Status.Id.ToString(), newProject.Status.Id.ToString(), logType);
                //changes.Add($"<p><span><strong>Поле \"Статус\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.Status.Name}</br>&emsp;&emsp; Новое значение : {newProject.Status.Name}</p>");
            }
            if (oldProject.ContractNumber != newProject.ContractNumber)
            {
                actionLogDAL.Add(newProject.Id, userName, "Номер Договора", oldProject.ContractNumber, newProject.ContractNumber, logType);
                //changes.Add($"<p><span><strong>Поле \"Номер Договора\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.ContractNumber}</br>&emsp;&emsp; Новое значение : {newProject.ContractNumber}</p>");
            }
            if (!oldProject.CompanyId.Equals(newProject.CompanyId))
            {
                actionLogDAL.Add(newProject.Id, userName, "Заказчик", oldProject.CompanyId.ToString(), newProject.CompanyId.ToString(), logType);
                //changes.Add($"<p><span><strong>Поле \"Заказчик\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.Company.Name}</br>&emsp;&emsp; Новое значение : {newProject.Company.Name}</p>");
            }
            if (!oldProject.Cost.Equals(newProject.Cost))
            {
                actionLogDAL.Add(newProject.Id, userName, "Стоимость", oldProject.Cost.ToString(), newProject.Cost.ToString(), logType);
                //changes.Add($"<p><span><strong>Поле \"Стоимость\" :</strong></span> </br>&emsp;&emsp; Старое значение : {oldProject.Cost}</br>&emsp;&emsp; Новое значение : {newProject.Cost}</p>");
            }

            if (changes.Count != 0)
                {
                EmailBLL.ProjectChanged(newProject, changes);
            }
        }

        private string FieldChangeString(string fieldName, string oldValue, string newValue)
        {
            return $"<p><span><strong>Поле \"{fieldName}\" :</strong></span> </br>&emsp;&emsp; <b>Старое значение :</b></br>{oldValue}</br>&emsp;&emsp; <b>Новое значение :</b></br>{newValue}</p>";
        }

        public List<ActionLog> Read(int id, string type)
        {
            var result = actionLogDAL.Read(id, type);

            foreach (var log in result)
            {
                PrepareData(log);
                log.Account = Logic.Instance.Account.Get(log.UserLogin);
            }

            return result;
        }

        //Изменить данные, если в значениях хранятся ключи
        private void PrepareData(ActionLog log)
        {
            switch (log.FieldName)
            {
                case "Статус":
                    {
                        log.ChangedValue = Status.All[int.Parse(log.ChangedValue)].Name;
                        log.NewValue = Status.All[int.Parse(log.NewValue)].Name;
                        break;
                    }
                case "Исполнитель":
                    {
                        log.ChangedValue = Logic.Instance.Account.Get(log.ChangedValue).Name;
                        log.NewValue = Logic.Instance.Account.Get(log.NewValue).Name;
                        break;
                    }
                case "Заказчик":
                    {
                        log.ChangedValue = Logic.Instance.Company.Get(int.Parse(log.ChangedValue)).Name;
                        log.NewValue = Logic.Instance.Company.Get(int.Parse(log.NewValue)).Name;
                        break;
                    }
            }
        }

        private string DateFormat(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
    }
}