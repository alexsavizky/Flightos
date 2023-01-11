using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Flightos.Models;

namespace Flightos.Dal
{
    public class UserDal:DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admin");
        }
        public DbSet<Users> user { get; set; }
        public DbSet<Admin> Admin { get; set; }
    }
}