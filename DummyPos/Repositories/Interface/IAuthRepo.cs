using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IAuthRepo
    {
        /* bool Register(RegisterViewModel model);
         AppUser ValidateLogin(string email, string password);*/
        public LoggedInUser ValidateLogin(string email, string plainPassword);

        ////
        bool ChangePassword(string email, string oldPassword, string newPassword);
    }
}