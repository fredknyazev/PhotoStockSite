using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSite.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleAdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /RoleAdmin/
        public ActionResult UsersList()
        {
            var roles = db.Roles.ToList();
            var result = new List<RoleViewModel>();
            var users = db.Users.ToList();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            foreach (var role in roles)
            {
                var roleView = new RoleViewModel();
                roleView.Id = role.Id;
                roleView.Name = role.Name;
                roleView.UsersInRole = (from x in users where userManager.IsInRole(x.Id, role.Name) select x).ToList();
                result.Add(roleView);
            }

            return View(result);
        }
        [HttpPost]
        public ActionResult CreateRole(IdentityRole role)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            IdentityResult result = roleManager.Create(role);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList");
            }

            return View("~/Shared/Error");
        }
	}
}