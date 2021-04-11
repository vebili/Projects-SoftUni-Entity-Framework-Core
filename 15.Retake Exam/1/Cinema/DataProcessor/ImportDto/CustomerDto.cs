namespace Cinema.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class CustomerDto
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        [XmlElement(ElementName="FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        [XmlElement(ElementName="LastName")]
        public string LastName { get; set; }

        [Range(12, 110)]
        [XmlElement(ElementName="Age")]
        public int Age { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement(ElementName="Balance")]
        public decimal Balance { get; set; }

        [XmlArray(ElementName="Tickets")]
        public List<TicketDto> Tickets { get; set; }
    }
}