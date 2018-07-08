using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IMS.Models.Contexts;
using IMS.Models;


namespace IMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static bool IsAuthenticated;
        public static string UserRoll;
        public static string User;

        [STAThread]
        static void Main()
        {
            //Database.SetInitializer(new CustomDropCreateDatabaseIfModelChangesForUser());
            //Database.SetInitializer(new CustomDropCreateDatabaseIfModelChangesForUserData());


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

    }

    class CustomDropCreateDatabaseIfModelChangesForUser : DropCreateDatabaseAlways<LoginContext>
    {

        protected override void Seed(LoginContext context)
        {

            try
            {
                context.users.Add(new User() { Username = "Admin", Password = "12345", UserType = "Administrator" });
                context.SaveChanges();

                base.Seed(context);
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

    }

    class CustomDropCreateDatabaseIfModelChangesForUserData : DropCreateDatabaseAlways<UserDataContext>
    {

        protected override void Seed(UserDataContext context)
        {

            try
            {
                context.UsersData.Add(new UserData() { Username = "Admin", Password = "12345", UserType = "Administrator", FirstName = "Tharindu", LastName = "Senevirathna", Email = "tharindu73senevirathna@gmail.com" });
                context.SaveChanges();

                base.Seed(context);
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

    }

}
