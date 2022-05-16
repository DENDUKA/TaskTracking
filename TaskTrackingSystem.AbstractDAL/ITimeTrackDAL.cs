using System.Collections.Generic;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.AbstractDAL
{
    public interface ITimeTrackDAL
    {

        bool Delete(int id);

        int Create(TimeTrack timeTrack);

        bool Update(TimeTrack timeTrack);

        List<TimeTrack> GetAll();

        List<TimeTrack> GetAllForAccount(string account);
    }
}