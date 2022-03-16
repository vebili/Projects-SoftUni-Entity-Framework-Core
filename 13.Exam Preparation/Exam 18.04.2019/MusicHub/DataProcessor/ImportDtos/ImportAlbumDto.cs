using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MusicHub.Data.Models;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class ImportAlbumDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

    }
}
