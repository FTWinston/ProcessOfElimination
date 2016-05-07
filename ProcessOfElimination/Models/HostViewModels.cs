using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProcessOfElimination.Models
{
    [Validator(typeof(CreateValidator))]
    public class CreateViewModel
    {
        [Display(Name = "Game name")]
        public string Name { get; set; }
        
        [Display(Name = "Private game")]
        public bool Private { get; set; }
        
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Display(Name = "Number of players")]
        public int NumPlayers { get; set; }
    }

    public class CreateValidator : AbstractValidator<CreateViewModel>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Give your game a name to identify it.")
                .Length(3, 50).WithMessage("The name must be between 3 and 50 characters long..");

            RuleFor(x => x.Password).NotEmpty().When(x => x.Private).WithMessage("A private game must have a password.");
            
            RuleFor(x => x.NumPlayers)
                .NotEmpty().WithMessage("Please specify how many player slots your game will contain.")
                .InclusiveBetween(3, 12).WithMessage("A game must have between 3 and 12 player slots.");
        }
    }

}
