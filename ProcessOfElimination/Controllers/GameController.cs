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
    public class GameController : Controller
    {
        // GET: Game/Play/1
        public ActionResult Play(int id, int? day)
        {
            var db = new Entities();
            var game = db.Games.Single(g => g.ID == id);

            if (!game.HasStarted)
                return View("Lobby", new LobbyViewModel(game, User.Identity.GetUserId()));

            GameTurn turn;
            if (day.HasValue)
                turn = game.GameTurns.Single(t => t.Number == day);
            else
                turn = game.GameTurns.OrderByDescending(t => t.Number).First();

            return View("Play", new PlayViewModel(game, turn, User.Identity.GetUserId()));
        }

        // POST: /Game/Join
        [HttpPost]
        public ActionResult Join(JoinViewModel model)
        {
            var db = new Entities();
            Game game = db.Games.Single(g => g.ID == model.JoinInfo.GameID);

            if (game.HasStarted)
            {
                ModelState.AddModelError(string.Empty, "You cannot join this game, because it has already started.");
            }

            if (game.Password != null)
            {
                if (string.IsNullOrEmpty(model.JoinInfo.Password))
                    ModelState.AddModelError("JoinInfo.Password", "You must enter the password to join this game.");
                else if (!GameService.CheckPassword(model.JoinInfo.Password, game))
                    ModelState.AddModelError("JoinInfo.Password", "The password you entered is incorrect.");
            }

            if (!ModelState.IsValid)
                return View("Lobby", new LobbyViewModel(game, User.Identity.GetUserId(), model.JoinInfo));

            GameService.JoinGame(db, game, User.Identity.GetUserId(), model.JoinInfo.Name);

            if (game.GamePlayers.Count >= game.NumPlayers)
                GameService.Start(db, game);

            return RedirectToAction("Play", new { id = game.ID });
        }
    }
}