﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Post;
using Social_Network.Models.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Network.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ValidateUserSession _validateUserSession;

        public PostController(IPostService postService, ValidateUserSession validateUserSession)
        {
            _postService = postService;
            _validateUserSession = validateUserSession;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string description, IFormFile postImage = null)
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            SavePostViewModel savePostVm = new();
            savePostVm.PostDescription = description;
            savePostVm.PostImage = postImage;

            if (savePostVm.PostDescription != null)
            {
               savePostVm = await _postService.Add(savePostVm);
                if (savePostVm != null && savePostVm.Id != 0 && postImage != null)
                {
                    savePostVm.ImgUrl = UploadImages.UploadFile(savePostVm.Id, "Posts", new List<IFormFile> { postImage });
                    await _postService.Update(savePostVm, savePostVm.Id);
                }
            }
            else
            {
                TempData["postError"] = "You need to write something before posting this";
            }

            return RedirectToRoute(new { controller="Home", action="Index"});
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string description, DateTime dateCreated, IFormFile postImage = null)
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (description != null)
            {
                SavePostViewModel savePostVm = new();
                savePostVm.Id = id;
                savePostVm.PostDescription = description;
                savePostVm.PostImage = postImage;
                savePostVm.DateCreated = dateCreated;

                SavePostViewModel saveVm = await _postService.GetByIdSaveViewModel(id);
                savePostVm.ImgUrl = UploadImages.UploadFile(saveVm.Id, "Posts", new List<IFormFile> { postImage }, true, saveVm.ImgUrl);

                await _postService.Update(savePostVm, savePostVm.Id);
            }
            else
            {
                TempData["postError"] = "You need to write something before posting this";
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                ModelState.AddModelError("SecurityError", "You have no permission to access this link. Log in First");
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            await _postService.Delete(id);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
