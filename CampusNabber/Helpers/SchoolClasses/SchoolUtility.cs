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
            List<SchoolModel> result = new List<SchoolModel>();
            foreach(School s in db.Schools)
            {
                result.Add(SchoolModel.bindToSchoolModel(s));
            }
            return result;
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