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

         public List<PostItem> Posts { get; set; }
         public string school_name { get; set; }
         public int rangeFrom { get; set; }
        public int displayRange { get; set; }
        public int rangeTo { get; set; }

        //Christian Change
        // this needs to filter out the current user's PostItems
        public void setList(ApplicationUser user)
        {
            displayRange = 1;
            rangeFrom = 0;
            rangeTo = displayRange;
            //might want to change this to a school object in the future //Christian
            school_name = user.school_name;
            CNQuery query = new CNQuery("PostItem");
            query.setQueryWhereKeyEqualToCondition("school_name", user.school_name);
            query.setQueryWhereKeyNotEqualToCondition("username", user.UserName);
            Posts = query.select().Cast<PostItem>().ToList();
        }

        //increment the values by a higher factor to display more records per page
        public PostItem getPost(int index)
        {
            if (index < 0 || index > Posts.Count)
            {
                throw new Exception("This index is out of bounds");
            }
            
            return Posts.ElementAt(index);
        }

        public void incrimentRange()
        {
            rangeTo += displayRange;
            rangeFrom += displayRange;
        }

        public void decrimentRange()
        {
            rangeTo -= displayRange;
            rangeFrom -= displayRange;
        }
    }

    

   
}