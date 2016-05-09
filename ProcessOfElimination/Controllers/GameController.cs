using Microsoft.AspNet.Identity;
using ProcessOfElimination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProcessOfElimination.Controllers
{
    public class GameController : Controller
    {
        // GET: Game/Play/1
        public ActionResult Play(int id)
        {
            var db = new Entities();
            var game = db.Games.Single(g => g.ID == id);

            if (!game.HasStarted)
                return View("Lobby", new LobbyViewModel(game, User.Identity.GetUserId()));

            return View("Play", new PlayViewModel(game, User.Identity.GetUserId()));
        }
    }
}