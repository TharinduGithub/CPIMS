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
using IMS.Models.Contexts;
using System.Data.Entity;

namespace IMS
{
    public partial class Category : MetroForm
    {
        private string CurruntCode { get; set; }
        private bool IsValied { get; set; }
        private int selectedItemId { get; set; }

        public Category()
        {
            InitializeComponent();
            this.selectedItemId = 0;
        }

        private void Category_Load(object sender, EventArgs e)
        {
            ActionEnable();
            loadCategories();
        }

        public void ReCreate()
        {
            this.metroTextBox1.Clear();
            this.cat.Clear();
            this.IsValied = false;
            this.selectedItemId = 0;
            this.CurruntCode = null;
            cat.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            listBox1.SelectedItem = null;
            ActionEnable();
            ReCreate();
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            noti.Text = string.Empty;
            IsValied = true;

            if (checkCat(metroTextBox1.Text))
            {
                noti.Text = "This Code Is Allready Exits !";
                IsValied = false;
            }
        }

        public bool checkCat(string Val)
        {
            
            ItemContext context = new ItemContext();
            var Cat = context.Categories.Where(cat => cat.Code != CurruntCode).Where(ct=>ct.Code== Val).FirstOrDefault();
            if (Cat != null)
            {
                return true;
            }

            return false; 
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            

            if (listBox1.SelectedItem == null) return;
            ActionDisable();
            ItemContext context = new ItemContext();
            IMS.Models.Category C  = context.Categories.FirstOrDefault(cat => cat.Name == listBox1.SelectedItem.ToString());
            if (C != null)
            {
                this.selectedItemId = C.ID;
                this.CurruntCode = C.Code;
                metroTextBox1.Text = C.Code;
                cat.Text = C.Name;
            }
            noti.Text = string.Empty;
            

        }

        private void label3_Click(object sender, EventArgs e)
        {
            if(cat.Text != string.Empty && metroTextBox1.Text != string.Empty && IsValied == true)
            {
                saveNewCategory(new { Name = cat.Text, Code = metroTextBox1.Text });
            }
            
        }

        public void saveNewCategory(dynamic O)
        {
            if (this.IsValied==false)
            {
                MessageBox.Show("Please Change Code");
                return;
            }
            try
            {
                ItemContext context = new ItemContext();
                context.Categories.Add(new IMS.Models.Category() { Name = O.Name, Code = O.Code });
                context.SaveChanges();
                ReCreate();
                loadCategories();

            }
            catch(System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach(var errors in e.EntityValidationErrors)
                {
                    foreach(var error in errors.ValidationErrors)
                    {
                        MessageBox.Show(error.ErrorMessage);
                    }
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                if (e.InnerException != null)
                {
                    MessageBox.Show(e.InnerException.Message);
                }
            }
           
        }

        public async void loadCategories()
        {

            try
            {
                listBox1.Items.Clear();
                ItemContext context = new ItemContext();
                IEnumerable<Models.Category> Cats = await context.Categories.ToListAsync();
                foreach (Models.Category cat in Cats)
                {
                    listBox1.Items.Add(cat.Name);
                }
            }catch(Exception e){
                MessageBox.Show(e.Message);
                if (e.InnerException != null)
                {
                    MessageBox.Show(e.InnerException.Message);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (this.selectedItemId == 0)
            {
                MessageBox.Show("Please Select a Category to Delete !");
                return;
            }
            ItemContext context = new ItemContext();
            var Cat = context.Categories.FirstOrDefault(cat => cat.ID == this.selectedItemId);
            context.Categories.Remove(Cat);
            context.SaveChanges();
            ReCreate();
            loadCategories();
            

        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (this.selectedItemId == 0)
            {
                MessageBox.Show("Please Select a Category to Update !");
                return;
            }else if(this.IsValied == false)
            {
                MessageBox.Show("Please Change Code");
                return;
            }
            else
            {
                try
                {
                    ItemContext context = new ItemContext();
                    var Cat = context.Categories.FirstOrDefault(cat => cat.ID == this.selectedItemId);
                    Cat.Name = cat.Text;
                    Cat.Code = metroTextBox1.Text;
                    context.SaveChanges();
                    ReCreate();
                    loadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            

        }

        public void ActionEnable()
        {
            panel1.Enabled = true;
            panel2.Enabled = false;
            panel3.Enabled = false;
        }

        public void ActionDisable()
        {
            panel1.Enabled = false;
            panel2.Enabled = true;
            panel3.Enabled = true;
        }
    }
}
