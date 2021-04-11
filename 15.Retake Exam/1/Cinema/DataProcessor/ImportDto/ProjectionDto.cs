namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Projection")]
    public class ProjectionDto
    {
        [XmlElement(ElementName="MovieId")]
        public int MovieId { get; set; }

        [XmlElement(ElementName="DateTime")]
        public string DateTime { get; set; }
    }
}