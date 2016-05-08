using Microsoft.AspNet.Identity;
using ProcessOfElimination.Models;
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
            var db = new Entities();
            var games = db.Games.Where(g => g.HasStarted == false && g.Password == null).ToList();

            ViewBag.Message = "Browse all open games here";
            return View(games);
        }

        // GET: Browse/Mine
        public ActionResult Mine()
        {
            var db = new Entities();
            var userID = User.Identity.GetUserId();
            var games = db.Games.Where(g => g.HasFinished == false && (g.HostedByUserID == userID || g.GamePlayers.Where(gp => gp.UserID == userID).Any())).ToList();

            ViewBag.Message = "Browse all of your games here";
            return View("Index", games);
        }
    }
}