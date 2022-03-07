namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Collections.Generic;
    using Data.Models;
    using Newtonsoft.Json;

    public class GenresExportDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Games")]
        public List<GamesExportDto> Games { get; set; }

        [JsonProperty("TotalPlayers")]
        public int TotalPlayers { get; set; }
    }
}