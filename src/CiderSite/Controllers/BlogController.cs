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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
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
            return RedirectToAction("Profile", "Account");
        }

        public IActionResult Details(int id)
        {
            var thisBlog = _db.Blogs
                .Include(blogs => blogs.BlogComments)
                .FirstOrDefault(blogs => blogs.BlogId == id);
            return View(thisBlog);
        }

        //For resource Authorization
        //IAuthorizationService _authorizationService;
        //public BlogController(IAuthorizationService authorizationService)
        //{
        //    _authorizationService = authorizationService;
        //}
        //Task<bool> AuthorizeAsync(ClaimsPrincipal user,
        //                     object resource,
        //                     IEnumerable<IAuthorizationRequirement> requirements);

        //[Authorize]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    Blog thisBlog = _db.Blogs.FirstOrDefault(blogs => blogs.BlogId == id);
        //    if (thisBlog == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //    if (await _authorizationService.AuthorizeAsync(User, thisBlog, "EditPolicy"))
        //    {
        //        return View(thisBlog);
        //    }
        //    else
        //    {
        //        return new ChallengeResult();
        //    }
        //}

        [Authorize]
        public IActionResult Edit(int id)
        {
            var thisBlog = _db.Blogs.FirstOrDefault(blogs => blogs.BlogId == id);
            return View(thisBlog);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Blog blog, IFormFile Data)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            if (Data != null)
            {
                using (Stream filestream = Data.OpenReadStream())
                using (MemoryStream mstream = new MemoryStream())
                {
                    filestream.CopyTo(mstream);
                    blog.Data = mstream.ToArray();
                }
            }
            _db.Entry(blog).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Profile", "Account");
        }

        //[Authorize]
        //public IActionResult Edit(int id)
        //{
        //    var thisBlog = _db.Blogs.FirstOrDefault(blogs => blogs.BlogId == id);
        //    return View(thisBlog);
        //}

        //[HttpPost]
        //public IActionResult Edit(Blog blog, IFormFile Data)
        //{

        //    if (Data != null)
        //    {
        //        using (Stream filestream = Data.OpenReadStream())
        //        using (MemoryStream mstream = new MemoryStream())
        //        {
        //            filestream.CopyTo(mstream);
        //            blog.Data = mstream.ToArray();
        //        }
        //    }
        //    _db.Entry(blog).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return RedirectToAction("Profile", "Account");
        //}

        [Authorize]
        public IActionResult Delete(int id)
        {
            var thisBlog = _db.Blogs.FirstOrDefault(blogs => blogs.BlogId == id);
            return View(thisBlog);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var thisBlog = _db.Blogs.FirstOrDefault(blogs => blogs.BlogId == id);
            _db.Blogs.Remove(thisBlog);
            _db.SaveChanges();
            return RedirectToAction("Profile", "Account");
        }
    }
}
