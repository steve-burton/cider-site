using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CiderSite.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace CiderSite.Controllers
{
    public class RecipeCommentController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RecipeCommentController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        private ApplicationDbContext _db = new ApplicationDbContext();
        public IActionResult Create(int id)
        {
            ViewBag.RecipeId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(RecipeComment recipeComment, int recipeId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == userId);
            recipeComment.RecipeId = recipeId;
            recipeComment.AppUser = user;
            _db.RecipeComments.Add(recipeComment);
            _db.SaveChanges();
            return RedirectToAction("Details", "Recipe", new { id = recipeComment.RecipeId });
        }
    }
}
