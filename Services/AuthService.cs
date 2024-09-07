using Microsoft.EntityFrameworkCore.Infrastructure;
using SS1892.EPLPredictor.Handler;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Services
{
    public class AuthService:IAuthService
    {
        private readonly DBCtx _context;

        public AuthService(DBCtx context)
        {
            _context = context;

        }
        public bool AuthenticateUser(UserModel model)
        {
            var user = _context.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
            bool authenticated = false;
            var hashePwd = PasswordHandler.GetHashPassword(model.Password);
            
            if (user != null && user.IsActive )
            {
                var isPwdCorrect = PasswordHandler.CompareHash(hashePwd, user.Password);
                authenticated = isPwdCorrect && true;
            }
            return authenticated;

        }

        public bool RegisterUser(UserModel model)
        {
            bool registered = false;
            var user = _context.Users.Where(u => u.UserName == model.UserName && u.Password == model.Password).FirstOrDefault();
            if (user != null)//user with same UserId exist
            {
                registered = false;
            }
            else
            {
                //hash the password
                var hashedPwd = PasswordHandler.GetHashPassword(model.Password);
                model.Password= hashedPwd;
                model.IsActive = true;
                
                model.JoinedDate= DateTime.Now;
                
                _context.Users.Add(model);
                _context.SaveChanges();

                registered = true;
            }
            return registered;
            ;
        }

        public UserModel GetProfile(string userName)
        {
            var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            if (user != null)
            {
                //var userModel = new UserModel() { UserName = user.UserName, Name = user.Name };
                return user;
            }
            return new UserModel();
            
        }

        public bool CheckForAdminRole()
        {
            if (AuthModel.Role == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }





}

