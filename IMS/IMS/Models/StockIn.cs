using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    class StockIn
    {
        [Key]
        public int invoiceID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Supplier Supplier { get; set; }

        [Required]
        public int Quntity { get; set; }

        public List<Item> Items { get; set; }
    }
}
