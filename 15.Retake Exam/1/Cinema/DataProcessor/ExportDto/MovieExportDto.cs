namespace Cinema.DataProcessor.ExportDto
{
    using System.Collections.Generic;
    using Data.Models;
    using Newtonsoft.Json;

    public class MovieExportDto
    {
        [JsonProperty("MovieName")]
        public string MovieName { get; set; }

        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonProperty("TotalIncomes")]
        public string TotalIncomes { get; set; }

        [JsonProperty("Customers")]
        public List<CustomerMovieDto> Customers { get; set; }
    }
}