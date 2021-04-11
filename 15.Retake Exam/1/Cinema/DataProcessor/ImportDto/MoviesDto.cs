namespace Cinema.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class MoviesDto
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        public string Title { get; set; }

        public string Genre { get; set; }

        public string Duration { get; set; }

        [Range(1.0, 10.0)]
        public double Rating { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        public string Director { get; set; }
    }
}