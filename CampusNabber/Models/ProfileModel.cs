﻿using DatabaseCode.CNQueryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CampusNabber.Validation;


namespace CampusNabber.Models
{
    public class ProfileModel
    {
        public virtual List<PostItemModel> Posts { get; set; }
        public virtual ApplicationUser user { get; set; }
        public virtual string school_name { get; set; }
        public virtual Guid school_id { get; set; }
        private static CampusNabberEntities db = new CampusNabberEntities();

        /*
        [ExistingEmailValidator(ErrorMessage = "Email already exists")]
        public virtual string email { get; set; }
        [ExistingUsernameValidator(ErrorMessage = "Username already exists")]
        public virtual string username { get; set; }
        public virtual string school_name { get; set; }
        */

        public void getProfilePosts(ApplicationUser user)
        {
           List<PostItem> _posts = db.PostItems.Where(p => p.username == user.UserName).ToList<PostItem>();
            Posts = new List<PostItemModel>();
            foreach(PostItem post in _posts)
            {
                Posts.Add(PostItemModel.bindToModel(post));
            }
        }





    }
}