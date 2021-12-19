using System;
using System.ComponentModel.DataAnnotations;

namespace EDW.Domain.Models
{
    public class Activity : Entity
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }
    }
}
