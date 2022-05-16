using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.EFDAL.Context;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.EFDAL
{
    public class ProjectTypeDAL : IProjectTypeDAL
    {
        /*
        * Геолого-разведочные работы
        * Сейсморазведка
        * Подсчет запасов
        * Разработка
        * Другие проекты
        */
        public List<ProjectType> GetAll()
        {
            using (var db = new ProjectTypeContext())
            {
                return db.ProjectTypes
                     .ToList();
            }
        }

        public ProjectType Get(int id)
        {
            using (var db = new ProjectTypeContext())
            {
                return db.ProjectTypes
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }
    }
}