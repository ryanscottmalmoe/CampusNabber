using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CampusNabber.App_Start
{
    internal class RoleActions
    {
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
            //NOTE: will need to create a new admin email/login
            var appUser = new ApplicationUser
            {
                Email = "iamaudrey@eagles.ewu.edu",
                UserName = "iamaudrey@eagles.ewu.edu"
            };
            userResult = userManager.Create(appUser, "Pa$$w0rd");

            if(!userManager.IsInRole(userManager.FindByEmail("iamaudrey@eagles.ewu.edu").Id, "Admin"))
            {
                userResult = userManager.AddToRole(userManager.FindByEmail("iamaudrey@eagles.ewu.edu").Id, "Admin");
            }
        }
    }
}