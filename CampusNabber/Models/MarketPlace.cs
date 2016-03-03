using DatabaseCode.CNQueryFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Models
{
    public class MarketPlace
    {
        //public ApplicationUser CurrentUser { get; set; }

         public virtual List<PostItem> Posts { get; set; }
         public School school { get; set; }

        //Christian Change
        // this needs to filter out the current user's PostItems
        public void setList(ApplicationUser user)
        {
            CNQuery query = new CNQuery("PostItem");
            query.setQueryWhereKeyEqualToCondition("school_name", user.school_name);
            query.setClassName("PostItem");
            Posts = query.select().Cast<PostItem>().ToList();
        }

    }
}