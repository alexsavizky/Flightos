using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flightos.Models
{
    public partial class PayTemp
    {
        public Flights flight { get; set; }
        public Flights flightreturn { get; set; }
        public Users users { get; set; }
    }
}