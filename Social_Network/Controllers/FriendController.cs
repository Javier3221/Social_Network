using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Friend;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Models.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Controllers
{
    public class FriendController : Controller
    {
        private readonly IPostService _postService;
        private readonly IFriendService _friendService;
        private readonly IUserService _userService;
        private readonly ValidateUserSession _validateUserSession;
        public FriendController(IPostService postService, ValidateUserSession validateUserSession, IFriendService friendService, IUserService userService)
        {
           _postService = postService;
            _validateUserSession = validateUserSession;
            _friendService = friendService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
            }
            ViewData["Friends"] = await _friendService.GetAllViewModelWithInclude();
            return View(await _postService.GetAllFriendPosts());
        }

        public async Task<IActionResult> Create(int userId, string userName, string friendUserName)
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            UserViewModel friend = await _userService.FindUserByUserName(friendUserName);

            if (friend == null)
            {
                TempData["Error"] = "Can't find user. Try checking for spelling errors.";
            }
            else if (friend.Id == userId)
            {
                TempData["Error"] = "You can't be your own friend.";
            }
            else if (friend.FriendUserNames.Contains(userName))
            {
                TempData["Error"] = "This user is your friend already.";
            }
            else
            {
                FriendViewModel newFriend = new();
                newFriend.UserId = userId;
                newFriend.FriendsWith = friend.Id;
                await _friendService.Add(newFriend);
            }

            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            await _friendService.Delete(id);
            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }
    }
}
