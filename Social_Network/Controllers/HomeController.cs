using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Models;
using Social_Network.Models.Middlewares;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly ValidateUserSession _validateUserSession;

        public HomeController(ILogger<HomeController> logger, IPostService postService, ValidateUserSession validateUserSession)
        {
            _logger = logger;
            _postService = postService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (TempData["postError"] != null)
            {
                ViewBag.PostError = TempData["postError"].ToString();
            }
            else if (TempData["commentError"] != null)
            {
                ViewBag.CommentError = TempData["commentError"].ToString();
            }

            return View(await _postService.GetAllUserPosts());
        }
    }
}
