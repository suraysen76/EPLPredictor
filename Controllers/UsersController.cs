using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using SS1892.EPLPredictor.Services;

namespace SS1892.EPLPredictor.Controllers
{
    public class UsersController : Controller
    {
        private IUserService _userService;
        
        private IAuthService _authService;

        public UsersController(DBCtx context, IAuthService authService, IUserService userService)
        {            
            _authService = authService;
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();
            return View(users);
        }
       

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                //Authenticate User
                var registered = _authService.RegisterUser(model);
                if (registered)
                {
                    AuthModel.IsAuthenticated = true;
                    AuthModel.UserName = model.UserName;
                    AuthModel.Name = model.Name;
                    AuthModel.Role= model.Role;
                    return RedirectToAction("Index", "Home");

                }
                ViewBag.RegisterErrorMsg = "Register Failed";
                return View(model);

            }
            return View();
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            UserModel model = await _userService.GetUserById(id);
            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserModel userModel)
        {
            var retModel = await _userService.UpdateUsers(userModel);
            return RedirectToAction("Index");
        }

        //private bool UserModelExists(int id)
        //{
        //  return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
