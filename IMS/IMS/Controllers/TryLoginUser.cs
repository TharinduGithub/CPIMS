using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Models;
using System.Data.SqlClient;
using IMS.Models.Contexts;

namespace IMS.Controllers
{
    class TryLoginUser
    {
        public bool IsUser { get; }
        public string UserRoll { get;}

        public TryLoginUser() { }

        public TryLoginUser(User user)
        {
            LoginContext context = new LoginContext();
            
            User U = context.users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (U == null) return;

            if (U.UserType == "Administrator")
            {
                this.IsUser = true;
                this.UserRoll = "Administrator";

                Program.IsAuthenticated = true;
                Program.UserRoll = "Administrator";
                Program.User = U.Username;

            }
            else if (U.UserType == "User")
            {
                this.IsUser = true;
                this.UserRoll = "User";

                Program.IsAuthenticated = true;
                Program.UserRoll = "User";
                Program.User = U.Username;
            }
            else
            {
                Environment.Exit(0);
            }
            
        }


    }
}
