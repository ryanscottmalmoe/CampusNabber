using DatabaseCode.CNQueryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusNabber.Models
{
    public class ProfileModel
    {
        public virtual List<PostItem> Posts { get; set; }
        public virtual ApplicationUser user { get; set; }

        public void getProfilePosts(ApplicationUser user)
        {
            CNQuery query = new CNQuery("PostItem");
            query.setQueryWhereKeyEqualToCondition("username", user.UserName);
            Posts = query.select().Cast<PostItem>().ToList();
        }

    }
}