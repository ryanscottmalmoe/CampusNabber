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
        public int numPosts;
        public String SchoolToken { get; set; }
        public String[] CategoryNames { get; set; }
         public List<PostItem>[] Categories { get; set; }
         public List<PostItem> Posts { get; set; }
         public String school_name { get; set; }
         public List<String> SchoolsToDisplay { get; set; }
         public string user_name { get; set; }
         public int rangeFrom { get; set; }
         public int displayRange { get; set; }
         public int rangeTo { get; set; }
        public int? categoryToDisplay { get; set; }
        public string chosenCategory { get; set; }
        public string mainSchoolColor { get; set; }
        
        public MarketPlace(ApplicationUser user)
        {

            Categories = new List<PostItem>[4];
            SchoolsToDisplay = new List<String>();
            Posts = new List<PostItem>();
            CategoryNames = new String[] { "Automotive", "Books", "Housing", "Other" };
            numPosts = 0;
            displayRange = 2;
            rangeFrom = 0;
            rangeTo = displayRange;
            //might want to change this to a school object in the future //Christian
            school_name = user.school_name;
            SchoolsToDisplay.Add(school_name);
            user_name = user.UserName;
            SchoolToken = setSchoolToken(school_name);


        }

        public MarketPlace()
        {
           
        }
        
        public String setSchoolToken(String school_name)
        {
            String token = "";
            if (school_name.Equals("Eastern Washington University"))
                token += "[EWU]";
            else if (school_name.Equals("Washington State University"))
                token += "[WSU]";
            else if (school_name.Equals("Gonzaga"))
                token += "[GU]";
            else if (school_name.Equals("Whitworth"))
                token += "[WU]";
            return token;
        }
        public void  setCategoryNames()
        {
            CategoryNames = new String[] { "Automotive", "Books", "Housing", "Other" };
        }

        public void setList()
        {
            Categories = new List<PostItem>[4];
            Posts = new List<PostItem>();
            CategoryNames = new String[] { "Automotive", "Books", "Housing", "Other" };
            CNQuery query;
            Categories = new List<PostItem>[CategoryNames.Length];
            for (int i = 0; i < CategoryNames.Length; i++)
            {
                query = new CNQuery("PostItem");
                query.setQueryWhereKeyEqualToCondition("school_name", school_name);
                query.setQueryWhereKeyNotEqualToCondition("username", user_name);
                query.setQueryWhereKeyEqualToCondition("category", CategoryNames[i]);
                Categories[i] = query.select().Cast<PostItem>().ToList();
            }
           // int x = Categories[0].Count;
        }

        //increment the values by a higher factor to display more records per page
        public PostItem getPost(int index)
        {
            if (index < 0 || index > Posts.Count)
            {
                throw new Exception("This index is out of bounds");
            }
            else
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