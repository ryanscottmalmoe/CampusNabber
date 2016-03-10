using CampusNabber.Models;
using DatabaseCode.CNQueryFolder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CampusNabber.Helpers.SchoolClasses
{
    abstract public class SchoolFactory
    {
        /*This function will eventually build school names
        by querying a School database from school_name.
        That entry holds mainHexColor & secondaryHexColor
        and location coordinates for searches.
        */
        public static School BuildSchool(string school_name)
        {
            CNQuery schoolQuery = new CNQuery("School");
            schoolQuery.setQueryWhereKeyEqualToCondition("school_name", school_name);
            return schoolQuery.select().Cast<School>().First();
        }

        public static Color getColor(string hex)
        {
            return System.Drawing.ColorTranslator.FromHtml(hex);
        }

    }
}