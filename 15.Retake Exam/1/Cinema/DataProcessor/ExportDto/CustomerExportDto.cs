﻿namespace Cinema.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class CustomerExportDto
    {
        [XmlElement(ElementName="SpentMoney")]
        public string SpentMoney { get; set; }

        [XmlElement(ElementName="SpentTime")]
        public string SpentTime { get; set; }

        [XmlAttribute(AttributeName="FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute(AttributeName="LastName")]
        public string LastName { get; set; }
    }
}