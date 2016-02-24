using DatabaseCode.CNQueryFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusNabber.Models
{
    public class MarketPlace
    {
        //public ApplicationUser CurrentUser { get; set; }

         public virtual List<PostItem> Posts { get; set; }
         public School school { get; set; }
        CNQuery query { get; set; }
        //Christian Change
        // this needs to filter out the current user's PostItems
        public void setList(ApplicationUser user)
        {
          
            query = new CNQuery("PostItem");
            query.setQueryWhereKeyEqualToCondition("school_name", user.school_name);
            query.setQueryWhereKeyNotEqualToCondition("username", user.UserName);
           // query.setQ
            Posts= query.select().Cast<PostItem>().ToList();
       
        }
    }

    

   
}