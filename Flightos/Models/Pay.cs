
namespace Flightos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class Pay
    {

        [Required]
        public int? Id { get; set; }

        public string flightnum { get; set; }

        public int? uer_id { get; set; }

        public string card_num { get; set; }

        public int? card_ccv { get; set; }

        public string validity { get; set; }
        
    }
}