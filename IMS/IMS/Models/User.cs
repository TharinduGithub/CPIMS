using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models
{
   
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength =5)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(18, MinimumLength = 3)]
        public string UserType { get; set; }


    }
}
