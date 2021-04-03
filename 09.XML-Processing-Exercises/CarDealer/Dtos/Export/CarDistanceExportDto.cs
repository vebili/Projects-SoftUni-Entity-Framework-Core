﻿namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("car")]
    public class CarDistanceExportDto
    {
        [XmlElement(ElementName="make")]
        public string Make { get; set; }

        [XmlElement(ElementName="model")]
        public string Model { get; set; }

        [XmlElement(ElementName="travelled-distance")]
        public long Travelleddistance { get; set; }
    }
}