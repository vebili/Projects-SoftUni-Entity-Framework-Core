namespace TeisterMask.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40), MinLength(2)]
        public string Name { get; set; }

        public DateTime OpenDate { get; set; }
        public DateTime DueDate { get; set; }

        public ExecutionType ExecutionType { get; set; }
        public LabelType LabelType { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<EmployeeTask> EmployeesTasks { get; set; } = new HashSet<EmployeeTask>();
    }
}