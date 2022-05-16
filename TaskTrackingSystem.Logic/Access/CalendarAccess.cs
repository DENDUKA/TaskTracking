using System.Linq;
using System.Web;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.Logic.Access
{
    public class CalendarAccess
    {
        private string Login => HttpContext.Current.User.Identity.Name;
        private readonly string currentRole;

        public bool Edit { get; private set; }

        public CalendarAccess()
        {
            currentRole = Logic.Instance.Account.GetRole(Login);

            Edit = EditAccess();
        }

        private bool EditAccess()
        {
            return Role.AdmiBuhList.Contains(currentRole);
        }
    }
}
