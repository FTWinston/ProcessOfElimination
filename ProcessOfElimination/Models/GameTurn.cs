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
    
    public partial class GameTurn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GameTurn()
        {
            this.ChatMessages = new HashSet<ChatMessage>();
            this.PlayerActions = new HashSet<PlayerAction>();
        }
    
        public int ID { get; set; }
        public int GameID { get; set; }
        public int GameCardID { get; set; }
        public int Number { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Message { get; set; }
    
        public virtual GameCard GameCard { get; set; }
        public virtual Game Game { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerAction> PlayerActions { get; set; }
    }
}
