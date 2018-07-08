using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IMS.Models.Contexts
{
    class LoginContext : DbContext
    {
        public LoginContext() : base("name=ConnectionName") {}
        public DbSet<User> users { get; set; }
    }
}
