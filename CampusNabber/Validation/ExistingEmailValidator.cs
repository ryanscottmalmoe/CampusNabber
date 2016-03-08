using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusNabber.Validation
{
    public class ExistingEmailValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var email = value.ToString();
                // Create manager
                var manager = new UserManager<ApplicationUser>(
                   new UserStore<ApplicationUser>(
                       new ApplicationDbContext()));

                var user = manager.FindByEmail(email);
                if(user == null)
                     return true;
            }
            return false;
        }

    }
}