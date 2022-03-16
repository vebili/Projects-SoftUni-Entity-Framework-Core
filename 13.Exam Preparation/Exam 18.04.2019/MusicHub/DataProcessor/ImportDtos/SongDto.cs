namespace MusicHub.DataProcessor.ImportDtos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class SongDto
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }

        [XmlElement(ElementName="Duration")]
        public string Duration { get; set; }

        [XmlElement(ElementName="CreatedOn")]
        public string CreatedOn { get; set; }

        [XmlElement(ElementName="Genre")]
        public string Genre { get; set; }

        [XmlElement(ElementName="AlbumId")]
        public int? AlbumId { get; set; }

        [XmlElement(ElementName="WriterId")]
        public int WriterId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement(ElementName="Price")]
        public decimal Price { get; set; }
    }
}