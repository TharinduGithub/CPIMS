using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Models.Contexts;
using IMS.Models;
using System.Data.Entity;

namespace IMS.Controllers
{
    class TryManageItem
    {
        private Item Item;
        public TryManageItem(Item I)
        {
            this.Item = I;
        }

        public async Task<Item> InsertItem(int ID)
        {
           // try
            //{
                ItemContext context = new ItemContext();
                Item.Category = context.Categories.Where(c => c.ID == ID).First();
                context.Items.Add(Item);
                await context.SaveChangesAsync();
                return Item;
           // }
           // catch (Exception)
           // {
           //     return null;
           // }
        }

        public void Update(int ID)
        {
            try
            {
                ItemContext context = new ItemContext();

                var item =  context.Items.Where(i => i.ItemCode == Item.ItemCode).Single();
                item.ItemName = Item.ItemName;
                item.ReorderPoint = Item.ReorderPoint;
                item.OpenningStock = Item.OpenningStock;
                item.Category = context.Categories.Where(c => c.ID == ID).First();
                context.SaveChanges();

            }
            catch (Exception)
            {
                return;
            }

        }
    }
}
