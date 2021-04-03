namespace CarDealer.Dtos.Import
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Car")]
    public class CarImportDto
    {
        [XmlElement(ElementName="make")]
        public string Make { get; set; }

        [XmlElement(ElementName="model")]
        public string Model { get; set; }

        [XmlElement(ElementName="TraveledDistance")]
        public long TraveledDistance { get; set; }

        [XmlArray(ElementName="parts")]
        public List<PartIdDto> Parts { get; set; }
    }
}