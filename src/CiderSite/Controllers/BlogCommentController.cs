﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CiderSite.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CiderSite.Controllers
{
    public class BlogCommentController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public BlogCommentController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        private ApplicationDbContext _db = new ApplicationDbContext();
        public IActionResult Create(int id)
        {
            ViewBag.BlogId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogComment blogComment, int blogId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId);
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == userId);
            blogComment.BlogId = blogId;
            blogComment.AppUser = user;
            _db.BlogComments.Add(blogComment);
            _db.SaveChanges();
            return RedirectToAction("Details", "Blog", new { id = blogComment.BlogId });
        }
    }
}
         