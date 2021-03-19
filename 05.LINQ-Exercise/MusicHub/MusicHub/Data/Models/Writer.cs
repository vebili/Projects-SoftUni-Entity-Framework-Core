namespace MusicHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Writer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }
        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}