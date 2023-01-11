namespace Flightos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Flights
    {
        [Key]
        [StringLength(50)]
        public string flightnum { get; set; }

        [Required]
        public string flightcompany { get; set; }

        public int price { get; set; }

        public int seats { get; set; }

        [Required]
        public string from { get; set; }

        [Required]
        public string to { get; set; }

        [Required]
        public DateTime depdate { get; set; }

        [Required]
        public DateTime arrivedate { get; set; }

    }
}
