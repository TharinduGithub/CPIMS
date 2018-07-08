using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models
{
    class StockOut
    {
        [Key]
        public int invoiceID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Customer Customer { get; set; }

        [Required]
        public int Quntity { get; set; }

        public List<Item> Items { get; set; }
    }
}
