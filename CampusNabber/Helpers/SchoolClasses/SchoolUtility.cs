using CampusNabber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusNabber.Helpers.SchoolClasses
{
    abstract public class SchoolUtility
    {
        private static CampusNabberEntities db = new CampusNabberEntities();

        public static List<SchoolModel> generateSchools()
        {
           return new List<SchoolModel> {
                 new SchoolModel("Eastern Washington University", "EWU", "#A32020", "#FFFFFF"),
                 new SchoolModel("Washington State University", "WSU", "#DC143C", "#a1a0a0"),
                 new SchoolModel("Whitworth", "WU", "#DC143C", "#000000"),
                 new SchoolModel("Gonzaga", "GU", "#0F0FA9", "#A32020")
            };
        }

        //Save a list of generated schools
        public static void saveSchools(List<School> schools)
        {
            foreach (var item in schools)
            {
                db.Schools.Add(item);
            }
            db.SaveChanges();
        }
    }
}