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
using System.IO;
using System.ComponentModel.DataAnnotations;
using IMS.Controllers;

namespace IMS
{
    public partial class Edit : MetroForm
    {
        private Label L;
        private PictureBox P;

        public Edit(ref Label L, ref PictureBox P)
        {
            InitializeComponent();
            this.L = L;
            this.P = P;
        }

        private void Edit_Load(object sender, EventArgs e)
        {

            LoadData();

            
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            TxtPassword.PasswordChar = metroCheckBox1.Checked ? '\0' : '*';
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "Select Image |*.png; *.jpg; *.bmp";
            if (OP.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(OP.FileName);
            }
        }

        public async void LoadData()
        {
            try
            {

                UserDataContext context = new UserDataContext();
                UserData userData = await context.UsersData.SingleOrDefaultAsync(user=>user.Username==Program.User);
                if (userData == null) return;

                this.TxtFirstName.Text = userData.FirstName;
                this.TxtLastName.Text = userData.LastName;
                this.TxtEmail.Text = userData.Email;
                this.TxtUsername.Text = userData.Username;
                this.TxtUsername.Enabled = false;
                this.TxtPassword.Text = userData.Password;

                if(userData.Picture != null)
                {
                    MemoryStream MS = new MemoryStream(userData.Picture);
                    pictureBox1.Image = Image.FromStream(MS);
                }
                else
                {
                    pictureBox1.Image = null;
                }

                TxtFirstName.Focus();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                if (e.InnerException != null)
                {
                    MessageBox.Show(e.InnerException.Message);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MemoryStream MS;
            if (pictureBox1.Image != null)
            {
                MS = new MemoryStream();
                pictureBox1.Image.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            else
            {
                MessageBox.Show("Please Select an Image !");
                return;
            }

            UpdateUser(new UserData() {

                FirstName = TxtFirstName.Text,
                LastName = TxtLastName.Text,
                Email = TxtEmail.Text,
                Picture = MS.ToArray(),
                Username = TxtUsername.Text,
                Password = TxtPassword.Text                                           
            });
        }

        public void UpdateUser(UserData userData)
        {
            ValidationContext context = new ValidationContext(userData);
            List<ValidationResult> list = new List<ValidationResult>();

            if (!Validator.TryValidateObject(userData, context, list))
            {
                MessageBox.Show("Error !");
                return;
            }

            TryUpdateUser tryUser = new TryUpdateUser(userData);
            if (tryUser.Update())
            {
                
                L.Text = userData.FirstName;
                MemoryStream MS = new MemoryStream(userData.Picture);
                P.Image = Image.FromStream(MS);

                MessageBox.Show("Successfully Updated !");
            }

            



        }
    }
}
