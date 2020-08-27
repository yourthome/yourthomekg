using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class Infrastructure
    {
        public int InfrastructureID { get; set; }
        public int RentalID { get; set; }
        [JsonIgnore]
        public Rental Rental { get; set; }
        public bool? Cafe { get; set; } = null;
        public bool? Kindergarten { get; set; } = null;
        public bool? Parking { get; set; } = null;
        public bool? BusStop { get; set; } = null;
        public bool? Supermarket { get; set; } = null;
        public bool? Park { get; set; } = null;
        public bool? Hospital { get; set; } = null;
    }
}
