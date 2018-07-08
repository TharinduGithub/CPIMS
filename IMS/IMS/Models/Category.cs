using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(4,MinimumLength =4)]
        [Index("IX_Code", 1, IsUnique = true)]
        public string Code { get; set; }

        public List<Item> AllItems { get; set; }
    }
}
