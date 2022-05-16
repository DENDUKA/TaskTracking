using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using TaskTrackingSystem.Logic.Email.Templates;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Email
{
    public static class SendEmail
    {
        private static readonly string FromAddress = @"info@cgmnir.ru";
        private static readonly string SmtpAddress = @"smtp.mail.ru";
        private static readonly int Port = 587;
        private static readonly string Login = @"info@cgmnir.ru";
        private static readonly string Password = @"G1uduka";

        public static void TaskCreation(Task task, string toEmail)
        {
            if (IsValidEmail(toEmail))
            {
                var body = EmailTemplates.TaskCreate(task);
                try
                {
                    Send(body, "Новая задача", new MailAddress(toEmail));//TODO
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void TaskChanged(Task task, string toEmail, List<string> changes)
        {
            if (IsValidEmail(toEmail))
            {
                var body = EmailTemplates.TaskChanged(task, changes);
                try
                {
                    Send(body, "Задача обновлена", new MailAddress(toEmail));//TODO
                }
                catch (Exception ex)
                {

                }
            }
        }

        internal static void TaskEndTime(Task task, List<string> emailList)
        {
            foreach (var toEmail in emailList)
            {
                if (IsValidEmail(toEmail))
                {
                    var body = EmailTemplates.TaskEndTime(task);
                    //У исполнителя_задачи заканчивается срок выполнения по задаче название_задачи

                    try
                    {
#if !DEBUG
                        Send(body, "Окончание срока у задачи", new MailAddress(toEmail));
#endif

#if DEBUG
                        Send(body, "Окончание срока у задачи", new MailAddress("denis_zotkin@cgmnir.ru"));
#endif
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        internal static void OverdueProjectTime(Project project, List<string> emailList)
        {
            foreach (var toEmail in emailList)
            {
                if (IsValidEmail(toEmail))
                {
                    var body = EmailTemplates.OverdueProjectTime(project);

                    try
                    {
#if !DEBUG
                        Send(body, "Окончание срока у задачи", new MailAddress(toEmail));
#endif

#if DEBUG
                        Send(body, "Истекло время для Проекта", new MailAddress(toEmail));
#endif
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        internal static void OverdueTaskTimeToResp(Task task, List<string> emailList)
        {
            foreach (var toEmail in emailList)
            {
                if (IsValidEmail(toEmail))
                {
                    var body = EmailTemplates.OverdueTaskTimeToResp(task);

                    try
                    {
#if !DEBUG
                        Send(body, "Окончание срока у задачи", new MailAddress(toEmail));
#endif

#if DEBUG
                        Send(body, "Истекло время на задачу", new MailAddress(toEmail));
#endif
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        internal static void ProjectEndTime(Project project, List<string> emailList)
        {
            foreach (var toEmail in emailList)
            {
                if (IsValidEmail(toEmail))
                {
                    var body = EmailTemplates.ProjectEndTime(project);
                    //У исполнителя_задачи заканчивается срок выполнения по задаче название_задачи

                    try
                    {
#if !DEBUG
                        Send(body, "Окончание срока у задачи", new MailAddress(toEmail));
#endif

#if DEBUG
                        Send(body, "Окончание срока у проекта", new MailAddress("denis_zotkin@cgmnir.ru"));
#endif
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        internal static void ToAuthorTaskClosed(Task task, string toEmail)
        {
            if (IsValidEmail(toEmail))
            {
                var body = EmailTemplates.ToAuthorTaskClosed(task);

                try
                {
                    Send(body, "Задача завершена", new MailAddress(toEmail));
                }
                catch (Exception ex)
                {

                }
            }
        }

        internal static void ToResponsibleTaskClosed(Task task, string toEmail)
        {
            if (IsValidEmail(toEmail))
            {
                var body = EmailTemplates.ToResponsibleTaskClosed(task);

                try
                {
                    Send(body, "Задача завершена, требуется подтверждение", new MailAddress(toEmail));
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void ProjectCreation(Project project, string toEmail)
        {
            if (IsValidEmail(toEmail))
            {
                var body = EmailTemplates.ProjectCreate(project);
                try
                {
                    Send(body, "Новый проект", new MailAddress(toEmail));//TODO
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void ProjectChanged(Project project, string toEmail, List<string> changes)
        {
            if (IsValidEmail(toEmail))
            {
                var body = EmailTemplates.ProjectChanged(project, changes);
                try
                {
                    Send(body, "Проект изменён", new MailAddress(toEmail));//TODO
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static void Send(string body, string subject, MailAddress toEmail)
        {
            var from = new MailAddress(FromAddress, "TaskTracking");
            var m = new MailMessage(from, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            var smtp = new SmtpClient(SmtpAddress, Port)
            {
                Credentials = new NetworkCredential(Login, Password),
                EnableSsl = true
            };

            try
            {
                smtp.Send(m);
            }
            catch (Exception ex)
            {
            }
        }

        private static bool IsValidEmail(string email)
        {
            var pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
    }
}
