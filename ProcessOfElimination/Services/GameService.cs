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
        public static Game CreateGame(CreateViewModel model, string hostUserID)
        {
            var game = new Game();
            game.Name = model.Name;
            game.NumPlayers = model.NumPlayers;

            if (model.Private)
                game.Password = HashPassword(model.Password);

            game.HostedByUserID = hostUserID;
            game.HasStarted = false;
            game.HasFinished = false;
            game.CreatedOn = DateTime.Now;

            return game;
        }

        public static GamePlayer JoinGame(Game game, string currentUserID, string name)
        {
            GamePlayer player = new GamePlayer();
            player.Name = name;
            player.Game = game;
            player.Notes = string.Empty;
            player.Position = 0;
            player.PublicTeamID = player.PrivateTeamID = 1; // humans, default team
            player.UserID = currentUserID;

            game.GamePlayers.Add(player);
            
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
        }
        
        public static void ProcessEndOfTurn(Game game)
        {
            // end the current turn
            var currentTurn = game.GameTurns.OrderByDescending(gt => gt.Number).First();
            EndTurn(currentTurn);

            // either end the game or start the next turn
            if (game.ShouldEndGame())
                game.EndGame();
            else
            {
                var nextPlayer = game.GetNextPlayer(currentTurn.ActivePlayer);
                StartTurn(game, nextPlayer, currentTurn.Number + 1);
            }
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

        private static GamePlayer GetNextPlayer(this Game game, GamePlayer previous)
        {
            var next = game.GamePlayers.Where(gp => gp.Position > previous.Position).OrderBy(gp => gp.Position).FirstOrDefault();

            if (next == null)
                next = game.GamePlayers.OrderBy(gp => gp.Position).First();

            return next;
        }

        private static IEnumerable<GamePlayer> GetPlayersInOrder(this GameTurn turn)
        {
            var players = turn.Game.GamePlayers;
            foreach (var player in players.Where(gp => gp.Position >= turn.ActivePlayer.Position).OrderBy(gp => gp.Position))
                yield return player;

            foreach (var player in players.Where(gp => gp.Position <  turn.ActivePlayer.Position).OrderBy(gp => gp.Position))
                yield return player;
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

            // pick an event card for this turn
            turn.EventCard = game.DrawCards(true).First();
            turn.EventCard.Discarded = true;
            game.GameTurns.Add(turn);
        }

        private static void EndTurn(this GameTurn turn)
        {
            // TODO: resolve event

            foreach (var player in turn.GetPlayersInOrder())
            {
                // TODO: resolve any action played by this player
            }
            
            throw new NotImplementedException();
        }

        private static bool ShouldEndGame(this Game game)
        {
            // if no players left on human team, game should end
            if (!game.GamePlayers.Any(gp => gp.PrivateTeamID == 1))
                return true;

            // TODO: any win conditions for the human players?
            throw new NotImplementedException();
        }

        private static void EndGame(this Game game)
        {
            game.HasFinished = true;

            // TODO: well is there anything more to it than that, actually?
        }
    }
}