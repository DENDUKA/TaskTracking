using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;
using Z.EntityFramework.Plus;

namespace TaskTrackingSystem.EFDAL
{
    public class TaskDAL : ITaskDAL
    {
        public Task Get(int id)
        {
            using (var db = new TaskContext())
            {
                return db.Tasks
                     .Where(x => x.Id == id)
                     .Include(x => x.Status)
                     .Include(x => x.Project)
                     .Include(x => x.Project.ProjectType)
                     .Include(x => x.Account)
                     .Include(x => x.Reporter)
                     .FirstOrDefault();
            }
        }

        public List<Task> GetAllForProject(int projectId)
        {
            using (var db = new TaskContext())
            {
                return db.Tasks
                     .Where(x => x.ProjectId == projectId)
                     .Include(x => x.Status)
                     .Include(x => x.Project)
                     .Include(x => x.Account)
                     .Include(x => x.Reporter)
                     .ToList();
            }
        }

        public int Add(Task task)
        {
            using (var db = new TaskContext())
            {
                task.Account = null;
                task.Reporter = null;
                task.Status = null;

                db.Tasks.Add(task);
                db.SaveChanges();
            }

            return task.Id;
        }

        public OperationResult UpdateStatus(int id, int statusId)
        {
            using (var db = new TaskContext())
            {
                var task = db.Tasks.Where(x => x.Id == id)
                    .FirstOrDefault();

                if (task != null)
                {
                    task.StatusId = statusId;
                    db.SaveChanges();
                }
                else
                {
                    return new OperationResult(false, "Задача не найдена");
                }
            }

            return new OperationResult(true);
        }

        public bool UpdateMainFiels(Task task)
        {
            using (var db = new TaskContext())
            {
                db.Tasks.Attach(task);

                db.Entry(task).Property(x => x.Name).IsModified = true;
                db.Entry(task).Property(x => x.DateEnd).IsModified = true;
                db.Entry(task).Property(x => x.DateStart).IsModified = true;
                db.Entry(task).Property(x => x.StatusId).IsModified = true;
                db.Entry(task).Property(x => x.AccountLogin).IsModified = true;
                db.Entry(task).Property(x => x.StoryPoints).IsModified = true;
                db.Entry(task).Property(x => x.Description).IsModified = true;
                db.Entry(task).Property(x => x.IsApproved).IsModified = true;


                return db.SaveChanges() > 0;
            }
        }

        public List<Task> GetAllForAccount(string login)
        {
            using (var db = new TaskContext())
            {
                return db.Tasks
                     .Where(x => x.AccountLogin == login)
                     .Include(x => x.Project)
                     .Include(x => x.Account)
                     .Include(x => x.Status)
                     .Include(x => x.Reporter)
                     .ToList();
            }
        }

        public OperationResult Delete(int id)
        {
            using (var db = new TaskContext())
            {
                var res = db.Tasks
                     .Where(x => x.Id == id)
                     .Delete();
                return new OperationResult(res > 0);
            }
        }

        public List<Task> GetAll(DateTime start, DateTime end, int[] statusId)
        {
            using (var db = new TaskContext())
            {
                return db.Tasks
                     .Where(x => x.DateEnd >= start && x.DateStart <= end && statusId.Contains(x.StatusId))
                     .Include(x => x.Account)
                     .Include(x => x.Project)
                     .ToList();
            }
        }
    }
}