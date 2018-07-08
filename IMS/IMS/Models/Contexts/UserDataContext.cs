using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IMS.Models.Contexts
{
    class UserDataContext : DbContext
    {
        public UserDataContext() : base("name=ConnectionName") { }
        public DbSet<UserData> UsersData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .Map(map => {
                    map.Properties(p => new
                    {
                        p.ID,
                        p.Username,
                        p.Password,
                        p.UserType
                    });

                    map.ToTable("Users");
                })
                .Map(map =>{
                
                    map.Properties(p => new
                    {
                        p.FirstName,
                        p.LastName,
                        p.Email,
                        p.Picture
                    });

                    map.ToTable("UserDatas");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
