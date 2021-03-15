namespace MusicHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;

    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedOn { get; set; }
        public Genre Genre { get; set; }

        [ForeignKey("Album")]
        public int? AlbumId { get; set; }
        public Album Album { get; set; }

        [ForeignKey("Writer")]
        public int WriterId { get; set; }
        public Writer Writer { get; set; }

        
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        public IEnumerable<SongPerformer> SongPerformers { get; set; } = new HashSet<SongPerformer>();
    }
}