namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchaseExportDto
    {
        [XmlElement(ElementName="Card")]
        public string Card { get; set; }

        [XmlElement(ElementName="Cvc")]
        public string Cvc { get; set; }

        [XmlElement(ElementName="Date")]
        public string Date { get; set; }

        [XmlElement(ElementName="Game")]
        public GamePurchaseExportDto Game { get; set; }
    }
}