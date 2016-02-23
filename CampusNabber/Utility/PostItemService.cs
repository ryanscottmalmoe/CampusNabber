using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusNabber.Utility
{
    public class PostItemService
    {
        public PostItemService()
        {

        }


        public void setMissingFields(PostItem postItem, ApplicationUserManager userManager)
        {
            if(postItem.username == null)
            {
                throw new Exception();
            }
            postItem.post_date = System.DateTime.Today;
            var user = userManager.FindByName(postItem.username).school_name;
            postItem.school_name = userManager.FindByName(postItem.username).school_name;
            postItem.object_id = Guid.NewGuid();
            postItem.photo_path = "";
        }
    }
}