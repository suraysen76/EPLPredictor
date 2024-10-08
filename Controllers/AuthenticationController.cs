﻿using Microsoft.AspNetCore.Mvc;
using SS1892.EPLPredictor.Models;
using SS1892.EPLPredictor.Services;
using SS1892.EPLPredictor.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace SS1892.EPLPredictor.Controllers
{
    public class AuthenticationController : Controller
    {
        private IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel model)
        {
            
            if (ModelState.IsValid)
            {
                //Authenticate User
                var authenticated = _authService.AuthenticateUser(model);
                if (authenticated)
                {
                    var user = _authService.GetProfile(model.UserName);

                    AuthModel.IsAuthenticated = true;
                    AuthModel.Name = user.Name??"";
                    AuthModel.UserId = user.Id;
                    AuthModel.UserName = user.UserName??"";
                    AuthModel.Role = user.Role??"";
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.LoginErrorMsg = "Invalid UserName or Password";
                return View(model);

            }
            return View();
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
                    //HttpContext.Session.SetString("UserAuthenticated", "true");
                    //HttpContext.Session.SetString("UserName", model.UserName);

                    return RedirectToAction("Index", "Home");
                   
                }
                ViewBag.RegisterErrorMsg = "Register Failed";
                return View(model);

            }
            return View();
        }

        public IActionResult Logout()
        {
            AuthModel.IsAuthenticated = false;
            
            return View(model: AuthModel.UserName);
        }

        public IActionResult Profile()
        {
            var username = AuthModel.UserName;
            var userModel = _authService.GetProfile(username);
            return View(userModel);
        }
        public IActionResult ChangePassword()
        {
            var pmodel=new ChangePasswordModel() { UserName= AuthModel.UserName,NewPassword="",ConfirmPassword="" };
            return View(pmodel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                //fetch the User Details
                var userId = AuthModel.UserId;
                var userName = AuthModel.UserName;
                var user = _authService.GetProfile(userName);
                if (user == null)
                {
                    //If User does not exists, redirect to the Login Page
                    return RedirectToAction("Login", "Authentication");
                }
                // ChangePasswordAsync Method changes the user password
                var result = _authService.ChangePassword(model);
                
                if (!result)
                {
                    
                    return View();
                }
               
                return RedirectToAction("Profile", "Authentication");
            }
            return View(model);
        }


    }
}
