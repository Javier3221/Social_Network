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

            bool isAvailable = await _userService.FindUserNameAvailabilty(UserVm.UserName);

            if (!isAvailable)
            {
                ModelState.AddModelError("userNameValidation", "This User Name exists already");
                return View(UserVm);
            }

            SaveUserViewModel saveUserVm = await _userService.Add(UserVm);
            if (saveUserVm != null && saveUserVm.Id != 0)
            {
                saveUserVm.ProfileImgUrl = UploadImages.UploadFile(new List<IFormFile> { UserVm.ProfileImage }, saveUserVm.Id, "Profiles");
                await _userService.Update(saveUserVm, saveUserVm.Id);
            }
            
            
            return RedirectToRoute(new { controller = "User", action = "Login" });
        }
    }
}
