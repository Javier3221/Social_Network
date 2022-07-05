using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Models;
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

        public HomeController(ILogger<HomeController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
