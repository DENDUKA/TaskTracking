using System.Collections.Generic;
using System.Linq;
using TaskTrackingSystem.AbstractDAL;
using TaskTrackingSystem.DAL.LocalDAL;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL
{
    public class ProjectTypeLocalDAL : IProjectTypeDAL
    {
        /*
        * Геолого-разведочные работы
        * Сейсморазведка
        * Подсчет запасов
        * Разработка
        * Другие проекты
        */
        public ProjectType Get(int id)
        {
            return LocalStorage.ProjectTypes[id];
        }

        public List<ProjectType> GetAll()
        {
            return LocalStorage.ProjectTypes.Values.ToList();
        }
    }
}