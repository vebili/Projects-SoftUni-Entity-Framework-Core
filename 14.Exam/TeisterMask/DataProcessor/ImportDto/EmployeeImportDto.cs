namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class EmployeeImportDto
    {
        [Required]
        [MaxLength(40), MinLength(3)]
        [RegularExpression(@"^[A-Za-z]+[0-9]*")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(40), MinLength(3)]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }

        public List<int> Tasks { get; set; }
    }
}