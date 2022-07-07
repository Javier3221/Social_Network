using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Models.Middlewares;
using Social_Network.Core.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Social_Network.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly ValidateUserSession _validateUserSession;

        public UserController(IUserService userService, ValidateUserSession validateUserSession)
        {
            _userService = userService;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Login()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            UserViewModel userVm = await _userService.Login(loginVm);
            if (userVm != null)
            {
                if (userVm.ActivatedAccount == false)
                {
                    ModelState.AddModelError("userValidation", "You need to activate your account before Logging in. Check your email for the activation link.");
                    return View(loginVm);
                }

                HttpContext.Session.Set<UserViewModel>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userValidation", "Incorrect Username or Password");
            }

            return View(loginVm);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel UserVm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(UserVm);
            }

            bool nameIsAvailable = await _userService.FindUserNameAvailabilty(UserVm.UserName);
            SaveUserViewModel emailVerification = await _userService.FindUserByEmail(UserVm.Email);

            if (!nameIsAvailable && emailVerification != null) 
            {
                ModelState.AddModelError("userNameValidation", "This User Name exists already");
                ModelState.AddModelError("emailVerification", "This email is already in use. Choose another one");
                return View(UserVm);
            }
            else if (emailVerification != null)
            {
                ModelState.AddModelError("emailVerification", "This email is already in use. Choose another one");
                return View(UserVm);
            }
            else if (!nameIsAvailable)
            {
                ModelState.AddModelError("userNameValidation", "This User Name exists already");
                return View(UserVm);
            }

            SaveUserViewModel saveUserVm = await _userService.Add(UserVm);
            if (saveUserVm != null && saveUserVm.Id != 0)
            {
                saveUserVm.ProfileImgUrl = UploadImages.UploadFile(saveUserVm.Id, "Profiles", new List<IFormFile> { UserVm.ProfileImage });
                await _userService.Update(saveUserVm, saveUserVm.Id);
            }
            
            
            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        public IActionResult ActivateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAccount(string email)
        {
            SaveUserViewModel user = await _userService.FindUserByEmail(email);
            if (user == null)
            {
                ModelState.AddModelError("emailNotFound", "This account doesn't exist or has already been activated. Please check for spelling errors.");
                return View();
            }

            await _userService.ActivateAccount(user);
            ViewBag.isPost = true;
            return View();
        }
    }
}
