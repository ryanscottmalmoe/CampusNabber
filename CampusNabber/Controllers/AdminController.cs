using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [Authorize(Roles ="Admin")]
        public ActionResult AdminTools()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult BlockUser()
        {
            return View();
        }
	}
}