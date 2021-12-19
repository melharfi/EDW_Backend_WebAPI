using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDW.Domain.Models
{
    public class User : Entity
    {       
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string ActivityCode { get; set; }

        public long LastModifiedActivityTimeStamp { get; set; }
    }
}
