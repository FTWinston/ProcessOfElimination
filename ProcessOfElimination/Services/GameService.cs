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
            player.Position = 0;
            player.PublicTeamID = player.PrivateTeamID = 1; // humans, default team
            player.UserID = currentUserID;

            db.GamePlayers.Add(player);
            db.SaveChanges();
            
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

            game.LoadCards(db);
            var firstPlayer = game.SetupPlayers();
            game.StartTurn(firstPlayer, 1);

            db.SaveChanges();

            // TODO: send notification email?
        }
        
        private static void StartNextTurn(Entities db, Game game)
        {
            // end the current turn, start the next
            throw new NotImplementedException();
        }

        private static void LoadCards(this Game game, Entities db)
        {
            var cards = new List<GameCard>();

            foreach (var card in db.Cards)
                for (var i = 0; i < card.NumberPerGame; i++)
                {
                    var gc = new GameCard();
                    gc.Game = game;
                    gc.Card = card;
                    gc.Discarded = false;
                    cards.Add(gc);
                }

            cards.Shuffle();
            for (var i = 0; i < cards.Count; i++)
                cards[i].Position = i + 1;

            db.GameCards.AddRange(cards);
            db.SaveChanges();
        }
        
        private static IEnumerable<GameCard> DrawCards(this Game game, bool eventCards, int number = 1)
        {
            var cards = game.GameCards
                .Where(gc => !gc.Discarded && gc.Card.IsEvent == eventCards && gc.GamePlayer == null)
                .OrderByDescending(gc => gc.Position)
                .Take(number);

            foreach (var card in cards)
                yield return card;

            var remaining = number - cards.Count();
            if (remaining == 0)
                yield break;

            // re-shuffle the discarded cards, then draw as many extra as are necessary
            var discardPile = game.GameCards
                .Where(gc => gc.Discarded && gc.Card.IsEvent == eventCards)
                .ToArray();

            discardPile.Shuffle();
            for (var i = 0; i < discardPile.Length; i++)
            {
                var card = discardPile[i];
                card.Discarded = false;
                card.GamePlayer = null;
                card.Position = discardPile.Length - i;
            }

            cards = discardPile.Take(remaining);
            foreach (var card in cards)
                yield return card;

            remaining -= cards.Count();
            if (remaining > 0)
                throw new ArgumentOutOfRangeException("number", string.Format("Tried to draw {0} {1} cards, but came up {2} short, even after shuffling the discards.", number, eventCards ? "event" : "non-event", remaining));
        }

        private static GamePlayer SetupPlayers(this Game game)
        {
            // sort the players
            var players = game.GamePlayers.ToArray();
            players.Shuffle();

            // deal cards to the players, based on (naively) giving them one card per turn
            for (var i = 0; i < players.Length; i++)
            {
                var player = players[i];
                player.Position = i + 1;
                
                var num = 0;
                foreach (var card in game.DrawCards(false, i))
                {
                    card.GamePlayer = player;
                    card.Position = ++num; // position in player's hand
                }
            }

            return players.First();
        }

        private static void StartTurn(this Game game, GamePlayer player, int turnNumber)
        {
            // deal cards to this player, bringing them up to the number of players in the game
            var num = 0;
            var cardsNeeded = game.GamePlayers.Count - player.GameCards.Count;

            foreach (var card in game.DrawCards(false, cardsNeeded))
            {
                card.GamePlayer = player;
                card.Position = ++num; // position in player's hand
            }

            GameTurn turn = new GameTurn();
            turn.Game = game;
            turn.ActivePlayer = player;
            turn.Number = turnNumber;
            turn.Timestamp = DateTime.Now;
            turn.Message = "This is the first turn of the game. This text should probably come from the event card or something!";

            turn.EventCard = game.DrawCards(true).First(); // TODO: fix me! nothing to stop this card being drawn for a second time
            game.GameTurns.Add(turn);
        }

        private static void EndTurn(this GameTurn turn)
        {
            throw new NotImplementedException();
        }
    }
}