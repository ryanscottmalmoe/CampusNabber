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
        //Christian Change
        // this needs to filter out the current user's PostItems
        public void setList(ApplicationUser user)
        {
            CNQuery query = new CNQuery("PostItem");
            query.setQueryWhereKeyEqualToCondition("school_name", user.school_name);
            query.setClassName("PostItem");
            List<dynamic> list = query.select();
            Posts = new List<PostItem>(list.Count);
            foreach(dynamic d in list)
            {
                Posts.Add(new PostItem
                {
                    username = d.username,
                    category = d.category,
                    description = d.description,
                    object_id = d.object_id,
                    photo_path = d.photo_path,
                    post_date = d.post_date,
                    price = d.price,
                    school_name = d.school_name,
                    title = d.title
                });
            }
        }
    }

    

   
}