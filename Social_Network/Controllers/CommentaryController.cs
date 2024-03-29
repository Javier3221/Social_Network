﻿using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Commentary;
using Social_Network.Models.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Controllers
{
    public class CommentaryController : Controller
    {
        private readonly ICommentaryService _commentaryService;
        private readonly ValidateUserSession _validateUserSession;

        public CommentaryController(ICommentaryService commentaryService, ValidateUserSession validateUserSession)
        {
            _commentaryService = commentaryService;
            _validateUserSession = validateUserSession;
        }
        public async Task<IActionResult> Create(string description, int postId, bool isFriend = false)
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            CommentaryViewModel comment = new();
            comment.Description = description;
            comment.PostId = postId;

            if (comment.Description != null)
            {
                await _commentaryService.Add(comment);
            }
            else
            {
                TempData["commentError"] = "You need to write something before posting a comment";
            }

            if (isFriend)
            {
                return RedirectToRoute(new { controller = "Friend", action = "Index" });
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
