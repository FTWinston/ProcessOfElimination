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

        public static GamePlayer JoinGame(Entities db, Game game, string currentUserID, string name)
        {
            GamePlayer player = new GamePlayer();
            player.Name = name;
            player.Game = game;
            player.Notes = string.Empty;
            player.PublicTeamID = player.PrivateTeamID = 1; // humans, default team
            player.UserID = currentUserID;

            db.GamePlayers.Add(player);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return player;
        }

        public static bool CheckPassword(string password, Game game)
        {
            return HashPassword(password) == game.Password;
        }

        private static string HashPassword(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static void Start(Entities db, Game game)
        {
            game.HasStarted = true;
            game.GamePlayers.PickRandom().PrivateTeamID = 2; // secretly change one player onto the aliens team

            db.SaveChanges();

            throw new NotImplementedException();
        }
    }
}