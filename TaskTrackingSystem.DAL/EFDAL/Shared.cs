using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackingSystem.DAL.EFDAL
{
    public static class Shared
    {
        internal static readonly EFBD.TaskTrackingSystemContext Context = new EFBD.TaskTrackingSystemContext();
    }
}
