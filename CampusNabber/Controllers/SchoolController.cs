using CampusNabber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Controllers
{
    public class SchoolController : Controller
    {
        private static CampusNabberEntities db = new CampusNabberEntities();
        // GET: School
        public ActionResult AddSchool()
        {
            return View();
        }
    }
}