using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface IAuthService
    {
        bool AuthenticateUser(UserModel model);
        bool RegisterUser(UserModel model);
        public UserModel GetProfile(string userName);

        bool CheckForAdminRole();

    }
}
