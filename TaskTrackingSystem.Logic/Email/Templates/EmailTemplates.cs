using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Email.Templates
{
  public static   class EmailTemplates
    {
        public static string TaskCreate(Task task)
        {
            var description = task.Description.Replace("\n", "<br/>");

            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>На Вас создана новая задача в проекте '{task.Project.Name}' в категории '{task.Project.ProjectType.Name}'.</span></h2>
                    <p><span><strong>Задача:</strong></span> <a href='http://188.235.146.23/TaskTracking/Task/Details/{task.Id}'>{task.Name}</a></p>
                    <p><span><strong>Срок исполнения:</strong></span> {task.DateStart:dd.MM.yyy} - {task.DateEnd.Date:dd.MM.yyy}</p>
                    <p><span><strong>Статус задачи:</strong></span> {task.Status.Name}</p>
                    <p><span><strong>Описание:</strong>
                    </span> {description}</p>
                </div>";
        }

        internal static string TaskChanged(Task task, List<string> changes)
        {
            var c = "";
            foreach (var x in changes)
            {
                c += x;
            }

            var acc = Logic.Instance.Account.Get(HttpContext.Current.User.Identity.Name);

            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span><a href='http://188.235.146.23/TaskTracking/Account/Details/{HttpContext.Current.User.Identity.Name}'>{acc.Name}</a> внес следующие изменения в задачу <a href='http://188.235.146.23/TaskTracking/Task/Details/{task.Id}'>{task.Name}</a> в проекте {task.Project.Name}</span></h2>
                    {c}
                </div>";
        }

        internal static string TaskEndTime(Task task)
        {
            //У исполнителя_задачи заканчивается срок выполнения по задаче название_задачи
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>У исполнителя <a href='http://188.235.146.23/TaskTracking/Account/Details/{task.Account.Login}'>{task.Account.Name}</a> задачи <a href='http://188.235.146.23/TaskTracking/Task/Details/{task.Id}'>{task.Name}</a> заканчивается срок выполнения. Осталось {(DateTime.Today - task.DateEnd).Days} дней.</span></h2>
                </div>";
        }

        internal static string OverdueProjectTime(Project project)
        {
            //"Исполнитель_проекта просрочил выполнение проекта название_проекта"
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>Исполнитель проекта {Logic.Instance.Project.GetResponsiblesString(project)} просрочил выполнение проекта <a href='http://188.235.146.23/TaskTracking/Project/Details/{project.Id}'>{project.Name}</a>.</span></h2>
                </div>";
        }

        internal static string OverdueTaskTimeToResp(Task task)
        {
            //Исполнитель_задачи просрочил выполнение задачи название_задачи
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>Исполнитель задачи <a href='http://188.235.146.23/TaskTracking/Account/Details/{task.Account.Login}'>{task.Account.Name}</a> просрочил выполнение задачи <a href='http://188.235.146.23/TaskTracking/Task/Details/{task.Id}'>{task.Name}</a>.</span></h2>
                </div>";
        }

        internal static string ProjectEndTime(Project project)
        {
            //"У исполнителя_проекта заканчивается срок выполнения по проекту название_проекта"
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>У исполнителя {Logic.Instance.Project.GetResponsiblesString(project)} проекта <a href='http://188.235.146.23/TaskTracking/Project/Details/{project.Id}'>{project.Name}</a> заканчивается срок выполнения. Осталось {(DateTime.Today - project.DateEnd).Days} дней.</span></h2>
                </div>";
        }

        internal static string ToResponsibleTaskClosed(Task task)
        {
            //Исполнитель_задачи выполнил задачу название_задачи. Просьба зайти в данную задачу и поставить отметку о выполнении/невыполнении данной задачи
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>Исполнитель задачи <a href='http://188.235.146.23/TaskTracking/Account/Details/{task.Account.Login}'>{task.Account.Name}</a> выполнил задачу <a href='http://188.235.146.23/TaskTracking/Task/Details/{task.Id}'>{task.Name}</a>, просьба зайти в данную задачу и подтвердить её выполнение.</span>
                </div>";
        }

        internal static string ToAuthorTaskClosed(Task task)
        {
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>Задача <a href='http://188.235.146.23/TaskTracking/Task/Details/{task.Id}'>{task.Name}</a>, которую вы создали - <b>Завершена</b></span>
                </div>";
        }

        internal static string ProjectChanged(Project project, List<string> changes)
        {
            var c = "";
            foreach (var x in changes)
            {
                c += x;
            }

            var acc = Logic.Instance.Account.Get(HttpContext.Current.User.Identity.Name);

            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span><a href='http://188.235.146.23/TaskTracking/Account/Details/{HttpContext.Current.User.Identity.Name}'>{acc.Name}</a> внес следующие изменения в проект <a href='http://188.235.146.23/TaskTracking/Project/Incex/{project.Id}'>{project.Name}</a> в категории проектов {project.ProjectType.Name}</span></h2>
                    {c}
                </div>";
        }

        public static string ProjectCreate(Project project)
        {
            return $@"
                <style>.active-area{{background - color: #ddd; border: 1px solid #bbb; min-height: 300px; position: relative; border-radius: .5em; overflow-x: auto; padding-left: 10px;}}p{{text - align: left;}}span{{color: #3c78b5;}}</style>
                <div class='active-area'>
                    <h2><span>На Вас создан новый проект в категории {project.ProjectType.Name}.</span></h2>
                    <p><span><strong>Проект:</strong></span> <a href='http://188.235.146.23/TaskTracking/Project/Index/{project.Id}'>{project.Name}</a></p>
                    <p><span><strong>Заказчик:</strong></span> {project.Company.Name}</p>
                    <p><span><strong>Номер договора:</strong></span> {project.ContractNumber}</p>
                    <p><span><strong>Срок исполнения:</strong></span> {project.DateStart:dd.MM.yyy} - {project.DateEnd.Date:dd.MM.yyy}</p>
                    <p><span><strong>Статус проекта:</strong></span> {project.Status.Name}</p>
                </div>";
        }
    }
}