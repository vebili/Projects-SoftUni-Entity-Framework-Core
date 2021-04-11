namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        public string Title { get; set; }

        public Genre Genre { get; set; }
        public TimeSpan Duration { get; set; }

        [Range(1.0, 10.0)]
        public double Rating { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();

    }
}
