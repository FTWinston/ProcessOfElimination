//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProcessOfElimination.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayerAction
    {
        public int ID { get; set; }
        public int GameTurnID { get; set; }
        public int GamePlayerID { get; set; }
        public int GameCardID { get; set; }
        public int Order { get; set; }
    
        public virtual GameCard GameCard { get; set; }
        public virtual GamePlayer GamePlayer { get; set; }
        public virtual GameTurn GameTurn { get; set; }
    }
}
