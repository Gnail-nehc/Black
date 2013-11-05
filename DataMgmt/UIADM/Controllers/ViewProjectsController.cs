using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UIADM.Controllers
{
    public class ViewProjectsController : Controller
    {
        //
        // GET: /ViewProjects/
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Title = "Manage Project";
            return View();
        }

    }
}
