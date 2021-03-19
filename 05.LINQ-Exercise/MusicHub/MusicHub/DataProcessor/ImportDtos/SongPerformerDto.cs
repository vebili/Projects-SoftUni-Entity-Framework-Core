namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class SongPerformerDto
    {
        [XmlAttribute(AttributeName="id")]
        public int Id { get; set; }
    }
}