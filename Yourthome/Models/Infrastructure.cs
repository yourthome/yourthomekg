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
        public bool Cafe { get; set; }
        public bool Kindergarten { get; set; }
        public bool Parking { get; set; }
        public bool BusStop { get; set; }
        public bool Supermarket { get; set; }
        public bool Park { get; set; }
        public bool Hospital { get; set; }
    }
}
