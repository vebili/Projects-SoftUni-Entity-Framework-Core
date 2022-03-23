using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportBookDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlAttribute("Pages")]
        public int Pages { get; set; }

        [XmlElement("Date")]
        public string PublishedOn { get; set; }
    }
}
