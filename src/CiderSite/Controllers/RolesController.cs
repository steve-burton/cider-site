using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CiderSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CiderSite.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RolesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public IActionResult Index()
        {
            List<IdentityRole> roles = _db.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string roleName)
        {
            try
            {
                _db.Roles.Add(new IdentityRole()
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _db.SaveChanges();
                return RedirectToAction("Index", "Roles");
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Assign()
        {
            List<IdentityRole> roles = _db.Roles.ToList();
            return View(roles);
        }
        [HttpPost]
        public async Task<ActionResult> Assign(string userName, string userRole)
        {
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            var userResult = await _userManager.AddToRoleAsync(user, userRole);

            return RedirectToAction("Index");
        }
    }
}
