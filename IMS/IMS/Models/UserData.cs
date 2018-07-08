using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace IMS.Models
{

    public class UserData
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(18, MinimumLength = 3)]
        public string UserType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is Required !")]
        [StringLength(30,MinimumLength =3)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Email is Required !")]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is Required !")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalied Email !")]
        public string Email { get; set; }

        public byte[] Picture { get; set; }

    }
}
