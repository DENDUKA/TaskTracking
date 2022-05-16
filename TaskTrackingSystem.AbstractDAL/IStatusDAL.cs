using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface IStatusDAL
    {
        List<Status> GetAll();
        Status Get(int id);
        void ClearCache();
    }
}
