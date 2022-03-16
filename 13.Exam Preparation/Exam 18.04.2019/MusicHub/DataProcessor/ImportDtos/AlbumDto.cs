namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class AlbumDto
    {
        [Required]
        [MaxLength(40), MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string ReleaseDate { get; set; }
    }
}