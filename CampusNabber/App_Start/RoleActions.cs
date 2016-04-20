using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CampusNabber.Helpers.SchoolClasses;

namespace CampusNabber.App_Start
{
    internal class RoleActions
    {
        private static CampusNabberEntities db = new CampusNabberEntities();

        internal void AddAdminRoleAndUser()
        {
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult roleResult;
            IdentityResult userResult;

            //Now we create a role store object
            var roleStore = new RoleStore<IdentityRole>(context);

            var roleManager = new RoleManager<IdentityRole>(roleStore);

            //Create Admin role if it doesn't already exist
            if(!roleManager.RoleExists("Admin"))
            {
                roleResult = roleManager.Create(new IdentityRole { Name = "Admin" });
            }


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            /*
            (from o in context.PostItems
                                 where o.username.Equals(username)
                                 select o);
            */
            //NOTE: will need to create a new admin email/login
            var appUser = new ApplicationUser
            {
                Email = "campusnabber@gmail.com",
                UserName = "Admin",
                EmailConfirmed = true,
                school_id = (from d in db.Schools
                            where d.school_name.Equals("Eastern Washington University")
                            select d.object_id).First(),                 
                LockoutEnabled = true
                
            };
            userResult = userManager.Create(appUser, "Pa$$w0rd");

            if(!userManager.IsInRole(userManager.FindByEmail("campusnabber@gmail.com").Id, "Admin"))
            {
                userResult = userManager.AddToRole(userManager.FindByEmail("campusnabber@gmail.com").Id, "Admin");
            }
        }
    }
}