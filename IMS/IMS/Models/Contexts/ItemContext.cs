using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IMS.Models.Contexts
{
    class ItemContext : DbContext
    {
        public ItemContext() : base("name=ConnectionName") { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
