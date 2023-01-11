using Flightos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;



namespace Flightos.Dal
{
    public class FlightsDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Flights>().ToTable("Flights");
        }
        public virtual DbSet<Flights> Flights { get; set; }
    }
}