using Microsoft.AspNetCore.Mvc;
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
                    HttpContext.Session.SetString("UserAuthenticated", "true");
                    HttpContext.Session.SetString("UserName", model.UserName);

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
            var userName = AuthModel.UserName;
            var userModel = _authService.GetProfile(userName);
            return View(userModel);
        }

    }
}
