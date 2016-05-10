using FluentValidation;
using FluentValidation.Attributes;
using ProcessOfElimination.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public LobbyViewModel(Game game, string currentUserID, GameJoinInfo joinInfo = null)
            : base(game, currentUserID)
        {
            PlayersRemaining = Math.Max(0, game.NumPlayers - game.GamePlayers.Count);
            CanJoin = CurrentPlayer == null && PlayersRemaining > 0;

            if (joinInfo == null && CanJoin)
                joinInfo = new GameJoinInfo()
                {
                    GameID = game.ID,
                    Name = "",
                    Password = "",
                };

            JoinInfo = joinInfo;
        }

        public int PlayersRemaining { get; private set; }
        public bool CanJoin { get; private set; }
        public GameJoinInfo JoinInfo { get; set; }
    }

    public class JoinViewModel
    {
        public GameJoinInfo JoinInfo { get; set; }
    }
    
    [Validator(typeof(JoinValidator))]
    public class GameJoinInfo
    {
        public int GameID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class JoinValidator : AbstractValidator<GameJoinInfo>
    {
        public JoinValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("You must choose a name to use in this game.")
                .Length(3, 50).WithMessage("Your name must be between 3 and 50 characters long.");
        }
    }

    public class PlayViewModel : GameViewModel
    {
        public PlayViewModel(Game game, string currentUserID)
            : base(game, currentUserID)
        {
            
        }
    }
}