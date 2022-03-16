namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class ImportSongsDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }

        [XmlElement("CreatedOn")]
        public string CreatedOn { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlElement("AlbumId")]
        public int? AlbumId { get; set; }

        [XmlElement("WriterId")]
        public int WriterId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
//<Song>
//    <Name>What Goes Around</Name>
//    <Duration>00:03:23</Duration>
//    <CreatedOn>21/12/2018</CreatedOn>
//    <Genre>Blues</Genre>
//    <AlbumId>2</AlbumId>
//    <WriterId>2</WriterId>
//    <Price>12</Price>
//  </Song>
