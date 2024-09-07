using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SS1892.EPLPredictor.Handler;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using System.Runtime.CompilerServices;

namespace SS1892.EPLPredictor.Services
{
    public class UserService:IUserService
    {
        private readonly DBCtx _context;

        public UserService(DBCtx context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id).ToListAsync();
            return user.FirstOrDefault();
        }

       
        public async Task<List<UserModel>> GetUsers()
        {
            var users = await _context.Users.OrderBy(u => u.Name).ToListAsync();
            return users;
        }

        public async Task<UserModel> UpdateUsers(UserModel model)
        {
            var viewModel = _context.Users.Where(u => u.Id == model.Id).FirstOrDefault();
            viewModel.Name = model.Name;
            viewModel.Role = "Member";
            //hash the password
            var hashedPwd = PasswordHandler.GetHashPassword(model.Password);
            model.Password = hashedPwd;
            viewModel.IsActive = true;
            await _context.SaveChangesAsync();

            return viewModel;
        }
    }





}

