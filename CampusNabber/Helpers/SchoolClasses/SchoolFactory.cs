using CampusNabber.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CampusNabber.Helpers.SchoolClasses
{
    abstract public class SchoolFactory
    {

        private static CampusNabberEntities db = new CampusNabberEntities();

        /*This function will eventually build school names
        by querying a School database from school_name.
        That entry holds mainHexColor & secondaryHexColor
        and location coordinates for searches.
        */
        public static School BuildSchool(string school_name)
        {
            return db.Schools.Where(p => p.school_name == school_name).First();
        }

        public static Color getColor(string hex)
        {
            return System.Drawing.ColorTranslator.FromHtml(hex);
        }

    }
}