using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using IMS.Models.Contexts;
using IMS.Models;
using System.Data.Entity;
using System.IO;

namespace IMS
{
    public partial class Main : MetroForm
    {
        public Main()
        {
            //try
            //{
            //    Thread T1 = new Thread(() => {

            //        Application.Run(new Splash());

            //    });
            //    T1.Start();
            //    Thread.Sleep(3000);
            //    InitializeComponent();
            //    T1.Abort();
            //    Application.Run(new Login());
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    if (e.InnerException != null)
            //    {
            //        MessageBox.Show(e.InnerException.Message);
            //    }
            //}
            Program.IsAuthenticated = true;
            Program.UserRoll = "Administrator";
            Program.User = "Admin";
            InitializeComponent();



        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Program.IsAuthenticated)
            {
                this.User_Name.Text = Program.User;
                CreateProfile(Program.User);

                if (Program.UserRoll == "Administrator")
                {
                    this.Title.Text = "Captain Promotion IMS (Administrator)";
                }
                else if (Program.IsAuthenticated && Program.UserRoll == "User")
                {
                    this.Title.Text = "Captain Promotion IMS";

                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public async void CreateProfile(string User)
        {
            try
            {
                UserDataContext context = new UserDataContext();
                UserData userData = await context.UsersData.SingleOrDefaultAsync(user => user.Username == User);
                if (userData == null) return;
                Action A = () => {

                    User_Name.Text = userData.FirstName;
                    if (userData.Picture != null)
                    {
                        MemoryStream MS = new MemoryStream(userData.Picture);
                        pictureBox1.Image = Image.FromStream(MS);
                    }
                };
                BeginInvoke(A);
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

        private void logout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        public void Logout()
        {
            this.Close();
            try
            {
                Thread T1 = new Thread(delegate () {
                    Application.Run(new Main());
                });

                T1.SetApartmentState(ApartmentState.STA);
                T1.Start();

            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                if (e.InnerException != null)
                {
                    MessageBox.Show(e.InnerException.Message);
                }
            }
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            Label L = this.User_Name;
            PictureBox P = this.pictureBox1;

            Edit edit = new IMS.Edit(ref L, ref P);
            edit.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Category Cat = new Category();
            Cat.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Items I = new Items();
            I.ShowDialog();
        }
    }
}
