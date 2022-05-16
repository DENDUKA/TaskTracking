using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface IProjectTypeDAL
    {
       List<ProjectType> GetAll();
        ProjectType Get(int id);
    }
}
