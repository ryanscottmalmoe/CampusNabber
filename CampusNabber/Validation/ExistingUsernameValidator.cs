using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusNabber.Validation
{
    public class ExistingUsernameValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var username = value.ToString();
                // Create manager
                var manager = new UserManager<ApplicationUser>(
                   new UserStore<ApplicationUser>(
                       new ApplicationDbContext()));

                var user = manager.FindByName(username);
                if (user == null)
                    return true;
            }
            return false;
        }
    }
}