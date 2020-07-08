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
        public bool? Cafe { get; set; } = true;
        public bool? Kindergarten { get; set; } = true;
        public bool? Parking { get; set; } = true;
        public bool? BusStop { get; set; } = true;
        public bool? Supermarket { get; set; } = true;
        public bool? Park { get; set; } = true;
        public bool? Hospital { get; set; } = true;
    }
}
