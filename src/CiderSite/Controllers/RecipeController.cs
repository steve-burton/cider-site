using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CiderSite.Models;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace CiderSite.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RecipeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId = userId;

            List<Recipe> recipes = _db.Recipes.Where(ph => ph.User.Id == userId).ToList();
            return View(recipes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Author, string Title, string Ingredients, string Directions, string Notes)
        {
            Recipe newRecipe = new Recipe(Author, Title, Ingredients, Directions, Notes);

            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            newRecipe.User = currentUser;

            _db.Recipes.Add(newRecipe);
            _db.SaveChanges();
            return RedirectToAction("Index", "Recipe");
        }

        public IActionResult Details(int id)
        {
            var thisRecipe = _db.Recipes
                //.Include(recipes => recipes.RecipeComments)
                .FirstOrDefault(recipes => recipes.RecipeId == id);
            return View(thisRecipe);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.RecipeId = id;
            var thisRecipe = _db.Recipes.FirstOrDefault(recipes => recipes.RecipeId == id);
            return View(thisRecipe);
        }

        [HttpPost]
        public IActionResult Edit(Recipe recipe, int recipeId)
        {
            _db.Entry(recipe).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index", "Recipe");
        }
    }
}
