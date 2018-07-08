using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class Item
    {
        [Key]
        [Required]
        public string ItemCode { get; set; }

        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string ItemName { get; set; }

        public Category Category { get; set; }

        [Required]
        public int ReorderPoint { get; set; }

        [Required]
        public int OpenningStock { get; set; }

    }
}
