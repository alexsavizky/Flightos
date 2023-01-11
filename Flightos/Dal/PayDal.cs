using Flightos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Flightos.Dal
{
    public class PayDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pay>().ToTable("Pay");
        }
        public DbSet<Pay> Pay { get; set; }
    }
}