using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessOfElimination.Models
{
    public abstract class GameViewModel
    {
        public Game Game { get; protected set; }
    }

    public class LobbyViewModel : GameViewModel
    {
        public LobbyViewModel(Game game, string currentUserID)
        {
            Game = game;
        }
    }

    public class PlayViewModel : GameViewModel
    {
        public PlayViewModel(Game game, string currentUserID)
        {
            Game = game;
        }
    }
}