namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Card
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}\s+[0-9]{4}\s+[0-9]{4}\s+[0-9]{4}\s+$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"\b\d{3}\b")]
        public string Cvc { get; set; }

        public CardType Type { get; set; }
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}