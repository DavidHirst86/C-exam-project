using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exam.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Exam.Controllers
{
	public class HomeController : Controller
	{
		ApplicationDbContext context = new ApplicationDbContext();
		public ActionResult Index()
		{
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			string role = "Admin";
			if (!roleManager.RoleExists(role))
			{
				var roleresult = roleManager.Create(new IdentityRole(role));
			}
			// Fetching UserManager
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
			// Create User with UserManager
			var user = new ApplicationUser { UserName = "admin@mail.com", Email = "admin@mail.com" };
			var success = userManager.Create(user, "P_asswo0rd!");
			// Add a role to a User
			var result = userManager.AddToRole(user.Id, "Admin");

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

	
	}
}