namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Performer")]
    public class ImportPerformerDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string LastName { get; set; }

        [Range(18, 70)]
        public int Age { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public SongDto[] PerformersSongs { get; set; }
    }

    [XmlType("Song")]
    public class SongDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
//<Performer>
//    <FirstName>Peter</FirstName>
//    <LastName>Bree</LastName>
//    <Age>25</Age>
//    <NetWorth>3243</NetWorth>
//    <PerformersSongs>
//      <Song id = "2" />
//      < Song id="1" />
//    </PerformersSongs>
//  </Performer>
