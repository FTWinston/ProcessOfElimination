using Microsoft.AspNet.Identity;
using ProcessOfElimination.Models;
using ProcessOfElimination.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProcessOfElimination.Controllers
{
    public class HostController : Controller
    {
        // GET: Host
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var game = GameService.CreateGame(model, User.Identity.GetUserId());
            
            var db = new Entities();
            db.Games.Add(game);
            db.SaveChanges();

            return RedirectToAction("Play", "Game", new { id = game.ID });
        }
    }
}