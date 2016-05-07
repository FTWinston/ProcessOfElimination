using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProcessOfElimination.Controllers
{
    public class BrowseController : Controller
    {
        // GET: Browse
        public ActionResult Index()
        {
            return View();
        }

        // GET: Browse/Mine
        public ActionResult Mine()
        {
            return View();
        }
    }
}