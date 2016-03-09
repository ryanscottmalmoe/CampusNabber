using CampusNabber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusNabber.Helpers.SchoolClasses
{
    abstract public class SchoolUtility
    {
        public static List<School> generateSchools()
        {
           return new List<School> {
                 new School("Eastern Washingon University", "#A32020", "#FFFFFF"),
                 new School("Washington State University", "#DC143C", "#a1a0a0"),
                 new School("Whitworth", "#DC143C", "#000000"),
                 new School("Gonzaga", "#0F0FA9", "#A32020")
            };
        }

        //Save a list of generated schools
        public static void saveSchools(List<School> schools)
        {
            //School test = new School("Eastern Washingon University", "#A32020", "#FFFFFF");
            foreach (var item in schools)
            {
                item.createEntity();
            }
        }
    }
}