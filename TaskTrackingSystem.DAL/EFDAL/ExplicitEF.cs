using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.DAL.EFDAL
{
    public static class ExplicitEF
    {
        public static Account ToAccount(this EFBD.Account efAcc)
        {
            return null; //new Account(efAcc.Name, efAcc.Login, efAcc.Password, efAcc.Role, efAcc.Email, efAcc.pc);
        }

        public static EFBD.Account ToEF(this Account account)
        {
            return new EFBD.Account() { Email = account.Email, Login = account.Login, Name = account.Name, Password = account.Password, Role = account.Role };
        }
    }
}
