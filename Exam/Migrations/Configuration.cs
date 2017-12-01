namespace Exam.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
	using Exam.Models;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;

	internal sealed class Configuration : DbMigrationsConfiguration<Exam.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;

		}

        protected override void Seed(Exam.Models.ApplicationDbContext context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.

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
			userManager.Create(user, "P_asswo0rd");
			// Add a role to a User
			var result = userManager.AddToRole(user.Id, "Admin");
		}
    }
}
