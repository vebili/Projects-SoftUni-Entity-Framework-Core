namespace MusicHub.DataProcessor.ExportDtos
{
    using System.Collections.Generic;
    using Data.Models;
    using Newtonsoft.Json;

    public class AlbumExportDto
    {
        [JsonProperty("AlbumName")]
        public string AlbumName { get; set; }

        [JsonProperty("ReleaseDate")]
        public string ReleaseDate { get; set; }

        [JsonProperty("ProducerName")]
        public string ProducerName { get; set; }

        [JsonProperty("Songs")]
        public List<SongExportDto> Songs { get; set; }

        [JsonProperty("AlbumPrice")]
        public string AlbumPrice { get; set; }
    }
}