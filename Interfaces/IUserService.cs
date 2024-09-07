using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsers();
       
        Task<UserModel> GetUserById(int id);
        Task<UserModel> UpdateUsers(UserModel model);
        
    }
}
