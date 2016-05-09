using ProcessOfElimination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace ProcessOfElimination.Services
{
    public static class GameService
    {
        public static Game Create(CreateViewModel model, string hostUserID)
        {
            var db = new Entities();

            var game = new Game();
            game.Name = model.Name;
            game.NumPlayers = model.NumPlayers;

            if (model.Private)
                game.Password = HashPassword(model.Password);

            game.HostedByUserID = hostUserID;
            game.HasStarted = false;
            game.HasFinished = false;
            game.CreatedOn = DateTime.Now;

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }

        public static GamePlayer JoinGame(Game game, string currentUserID, string name)
        {
            var db = new Entities();

            GamePlayer player = new GamePlayer();
            player.Name = name;
            player.Game = game;
            player.UserID = currentUserID;

            db.GamePlayers.Add(player);
            db.SaveChanges();

            return player;
        }

        private static string HashPassword(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}