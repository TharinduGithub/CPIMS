using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    class Vendor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100,MinimumLength =3)]
        public string Name { get; set; }

        public List<Supplier> Suppliers { get; set; }
    }
}
