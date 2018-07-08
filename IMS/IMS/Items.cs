using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using IMS.Models;
using IMS.Models.Contexts;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using IMS.Controllers;
using System.Text.RegularExpressions;
using System.Threading;

namespace IMS
{
    public partial class Items : MetroForm
    {
        private int selectedItemId { get; set; }

        public Items()
        {
            InitializeComponent();
            this.selectedItemId = 0;
        }

        private void Items_Load(object sender, EventArgs e)
        {
            ActionEnable();
            LoadCategories();
            LoadCatsForSearch();
        }

        public async void LoadCatsForSearch()
        {

            try
            {
                ItemContext context = new ItemContext();
                var AllCats = await context.Categories.ToListAsync();
                if (AllCats == null) return;

                AllCats.Insert(0, new Models.Category { Name = "Select Category", ID = 0 });
                SearchByCat.DataSource = AllCats;
                SearchByCat.DisplayMember = "Name";
                SearchByCat.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void LoadCategories()
        {
            try
            {
                ItemContext context = new ItemContext();
                var AllCats = await context.Categories.ToListAsync();
                if (AllCats == null) return;

                AllCats.Insert(0, new Models.Category { Name = "Select Category", ID = 0 });
                bindingSource1.DataSource = AllCats;
                TCategory.DataSource = bindingSource1.DataSource;
                TCategory.DisplayMember = "Name";
                TCategory.ValueMember = "ID";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public string MakeNewItemCode(int CID)
        {

            ItemContext context = new ItemContext();
            try
            {
                var items = context.Items.Include(i => i.Category).Where(itm => itm.Category.ID == CID).ToList();
                if (items.Count > 0)
                {
                    var item = items.Last();
                    string resultString = Regex.Match(item.ItemCode, @"\d+").Value;

                    return item.Category.Code + (int.Parse(resultString) + 1).ToString().PadLeft(4, '0');

                }
                else
                {
                    var Cat = context.Categories.FirstOrDefault(cat => cat.ID == CID);
                    return Cat.Code + 1.ToString().PadLeft(4, '0');
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }

        }

        private async void TCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (panel1.Enabled == false)
            {
                return;
            }
            else if (TCategory.SelectedIndex <= 0)
            {
                ClearTextBoxes();
                return;
            }

            int CID = (int)TCategory.SelectedValue;
            string Code = await Task.Run<string>(() => MakeNewItemCode(CID));
            this.TItemCode.Text = Code.ToString();

        }

        public void ActionEnable()
        {
            panel1.Enabled = true;
            panel2.Enabled = false;
            panel3.Enabled = false;
            TCategory.Enabled = true;
        }

        public void ActionDisable()
        {
            panel1.Enabled = false;
            panel2.Enabled = true;
            panel3.Enabled = true;
            TCategory.Enabled = false;
        }

        public void ClearTextBoxes()
        {
            TItemCode.Clear();
            TItemName.Clear();
            TReorderPoint.Clear();
            TOpeningStock.Clear();
            TCategory.SelectedIndex = 0;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            ReCreate();
            metroGrid1.Rows.Clear();
            ClearTextBoxes();
            ActionEnable();
        }

        public void ReCreate()
        {
            this.selectedItemId = 0;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            int ID = (int)TCategory.SelectedValue;
            if (ID == 0) return;

            saveNewItem(ID);

        }

        public async void saveNewItem(int ID)
        {
            if (TReorderPoint.Text == string.Empty || TOpeningStock.Text == string.Empty)
            {
                MessageBox.Show("Fill All Fields");
                return;
            }

            Item item = new Item()
            {

                ItemCode = TItemCode.Text,
                ItemName = TItemName.Text,
                ReorderPoint = int.Parse(TReorderPoint.Text),
                OpenningStock = int.Parse(TOpeningStock.Text)

            };

            ValidationContext context = new ValidationContext(item);
            List<ValidationResult> list = new List<ValidationResult>();

            if (!Validator.TryValidateObject(item, context, list))
            {
                MessageBox.Show("Error !");
                return;
            }

            TryManageItem Insert = new TryManageItem(item);
            Item i = await Insert.InsertItem(ID);
            
   
            ReCreate();
            ClearTextBoxes();
            if (i != null) setCurrentRow(i);

        }

        public void setCurrentRow(Item I)
        {
            DataGridViewRow Row = (DataGridViewRow)metroGrid1.Rows[0].Clone();
            Row.Cells[0].Value = I.ItemCode;
            Row.Cells[1].Value = I.ItemName;
            Row.Cells[2].Value = I.Category.Name;
            Row.Cells[3].Value = I.ReorderPoint;
            Row.Cells[4].Value = I.OpenningStock;
            Row.Cells[5].Value = I.Category.ID;
            metroGrid1.Rows.Add(Row);

        }



        private void TReorderPoint_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void TOpeningStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SearchByCode_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void SearchByCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SearchByCat.SelectedIndex == 0)
            {
                metroGrid1.Rows.Clear();
                return;
            }

            AddDataToGrid((int)SearchByCat.SelectedValue);

        }

        public async void AddDataToGrid(int ID)
        {
            try
            {
                ItemContext context = new ItemContext();
                IEnumerable<Item> items = await context.Items.Include(i => i.Category).Where(itm => itm.Category.ID == ID).ToListAsync();
                if (items == null) return;

                metroGrid1.Rows.Clear();

                foreach (var item in items)
                {
                    DataGridViewRow Row = (DataGridViewRow)metroGrid1.Rows[0].Clone();
                    Row.Cells[0].Value = item.ItemCode;
                    Row.Cells[1].Value = item.ItemName;
                    Row.Cells[2].Value = item.Category.Name;
                    Row.Cells[3].Value = item.ReorderPoint;
                    Row.Cells[4].Value = item.OpenningStock;
                    Row.Cells[5].Value = item.Category.ID;
                    metroGrid1.Rows.Add(Row);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (metroGrid1.Rows[e.RowIndex].Cells[0].Value == null) return;
            ActionDisable();

            var Row = metroGrid1.Rows[e.RowIndex];

            TCategory.SelectedIndex = (int)Row.Cells[5].Value;
            TItemCode.Text = Row.Cells[0].Value.ToString();
            TItemName.Text = Row.Cells[1].Value.ToString();
            TReorderPoint.Text = Row.Cells[3].Value.ToString();
            TOpeningStock.Text = Row.Cells[4].Value.ToString();

        }

        private void label4_Click(object sender, EventArgs e)
        {
            int ID = (int)TCategory.SelectedValue;
            if (ID == 0) return;

            UpdateItem(ID);
        }

        public void UpdateItem(int ID)
        {
            if (TReorderPoint.Text == string.Empty || TOpeningStock.Text == string.Empty)
            {
                MessageBox.Show("Fill All Fields");
                return;
            }

            Item item = new Item()
            {

                ItemCode = TItemCode.Text,
                ItemName = TItemName.Text,
                ReorderPoint = int.Parse(TReorderPoint.Text),
                OpenningStock = int.Parse(TOpeningStock.Text)

            };

            ValidationContext context = new ValidationContext(item);
            List<ValidationResult> list = new List<ValidationResult>();

            if (!Validator.TryValidateObject(item, context, list))
            {
                MessageBox.Show("Error !");
                return;
            }


            TryManageItem Update = new TryManageItem(item);

            Thread T1 = new Thread(() =>
            {
                Update.Update(ID);
            });

            T1.Start();
            ReCreate();
            ClearTextBoxes();
            T1.Join();
            AddDataToGrid((int)SearchByCat.SelectedValue);

        }

        private void label5_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        public  void DeleteItem()
        {
            if(TItemCode.Text == null)
            {
                MessageBox.Show("Please Select a Item to Delete !");
                return;
            }

            ItemContext context = new ItemContext();
            var item = context.Items.FirstOrDefault(itm=>itm.ItemCode == TItemCode.Text);
            context.Items.Remove(item);
            context.SaveChanges();
            ReCreate();
            ClearTextBoxes();
            AddDataToGrid((int)SearchByCat.SelectedValue);
        } 
    }
}
