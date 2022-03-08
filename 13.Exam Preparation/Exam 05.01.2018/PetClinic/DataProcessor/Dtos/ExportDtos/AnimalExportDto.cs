namespace PetClinic.DataProcessor.Dtos.ExportDtos
{
    using Newtonsoft.Json;

    public class AnimalExportDto
    {
        [JsonProperty("OwnerName")]
        public string OwnerName { get; set; }

        [JsonProperty("AnimalName")]
        public string AnimalName { get; set; }

        [JsonProperty("Age")]
        public int Age { get; set; }

        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("RegisteredOn")]
        public string RegisteredOn { get; set; }
    }
}