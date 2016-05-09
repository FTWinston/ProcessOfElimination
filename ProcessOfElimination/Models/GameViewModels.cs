using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessOfElimination.Models
{
    public abstract class GameViewModel
    {
        protected GameViewModel(Game game, string currentUserID)
        {
            Game = game;
            CurrentPlayer = game.GamePlayers.SingleOrDefault(gp => gp.UserID == currentUserID);
            PrivateGame = game.Password != null;
            IsHost = PrivateGame && currentUserID == game.HostedByUserID;
        }

        public Game Game { get; private set; }
        public GamePlayer CurrentPlayer { get; private set; }
        public bool PrivateGame { get; private set; }
        public bool IsHost { get; private set; }
    }

    public class LobbyViewModel : GameViewModel
    {
        public LobbyViewModel(Game game, string currentUserID)
            : base(game, currentUserID)
        {
            PlayersRemaining = Math.Max(0, game.NumPlayers - game.GamePlayers.Count);
        }

        public int PlayersRemaining { get; private set; }
    }

    public class PlayViewModel : GameViewModel
    {
        public PlayViewModel(Game game, string currentUserID)
            : base(game, currentUserID)
        {
            
        }
    }
}