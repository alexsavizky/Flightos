namespace Flightos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        public string fname { get; set; }

        [Required]
        public string lname { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
