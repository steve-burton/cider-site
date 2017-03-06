using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using CiderSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CiderSite.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public BlogController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        //public BlogController(ApplicationDbContext db)
        //{
        //    _db = db;
        //}


        public IActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string Author, string Title, string Intro, string BodyCopy, IFormFile Data)
        {
            byte[] photoArray = new byte[0];

            if (Data != null)
            {
                using (Stream fileStream = Data.OpenReadStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    photoArray = memoryStream.ToArray();
                }
            }

            Blog newBlog = new Blog(Author, Title, Intro, BodyCopy, photoArray);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            newBlog.User = currentUser;

            _db.Blogs.Add(newBlog);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
        {
            var thisBlog = _db.Blogs.FirstOrDefault(blogs => blogs.Id == id);
            return View(thisBlog);
        }
    }
}
