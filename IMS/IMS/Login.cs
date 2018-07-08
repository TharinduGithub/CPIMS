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
using IMS.Controllers;
using System.ComponentModel.DataAnnotations;

namespace IMS
{
    public partial class Login : MetroForm
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            metroTextBox1.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label3.Text = string.Empty;
            CheckUser(new User() { Username = metroTextBox1.Text, Password = metroTextBox2.Text });
            
        }

        public void CheckUser(User user)
        {
            try
            {
                ValidationContext context = new ValidationContext(user);
                List<ValidationResult> list = new List<ValidationResult>();

                if (!Validator.TryValidateObject(user, context, list))
                {
                    MessageBox.Show("Fillout All Fields !");
                    return;
                }

                TryLoginUser login = new TryLoginUser(user);
                if (login.IsUser)
                {
                    this.Dispose();
                }
                else
                {

                    this.metroTextBox1.Clear();
                    this.metroTextBox2.Clear();
                    metroTextBox1.Focus();
                    label3.Text = "Invalied Credentials !";

                }
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

        protected override void OnClosing(CancelEventArgs e)
        {
            Environment.Exit(0);
            base.OnClosing(e);
        }
    }
}
