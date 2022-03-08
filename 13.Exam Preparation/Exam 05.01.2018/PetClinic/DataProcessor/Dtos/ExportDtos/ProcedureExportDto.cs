namespace PetClinic.DataProcessor.Dtos.ExportDtos
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureExportDto
    {
        [XmlElement(ElementName="Passport")]
        public string Passport { get; set; }

        [XmlElement(ElementName="OwnerNumber")]
        public string OwnerNumber { get; set; }

        [XmlElement(ElementName="DateTime")]
        public string DateTime { get; set; }

        [XmlArray(ElementName="AnimalAids")]
        public List<AnimalAidExportDto> AnimalAids { get; set; }

        [XmlElement(ElementName="TotalPrice")]
        public decimal TotalPrice { get; set; }
    }
}