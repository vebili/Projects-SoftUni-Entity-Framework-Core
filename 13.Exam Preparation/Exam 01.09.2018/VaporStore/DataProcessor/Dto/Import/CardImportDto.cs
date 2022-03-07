namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Enums;
    using Newtonsoft.Json;

    public class CardImportDto
    {
        [Required]
        [RegularExpression(@"\b\d{4} \d{4} \d{4} \d{4}\b")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"\b\d{3}\b")]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}