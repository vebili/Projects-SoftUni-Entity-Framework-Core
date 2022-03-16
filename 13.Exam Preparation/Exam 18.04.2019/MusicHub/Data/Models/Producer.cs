namespace MusicHub.Data.Models
{
    using System.Collections.Generic;
    
    using System.ComponentModel.DataAnnotations;

    public class Producer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"\+359 \d{3} \d{3} \d{3}")]
        public string PhoneNumber { get; set; }
        public ICollection<Album> Albums { get; set; } = new HashSet<Album>();
    }
}