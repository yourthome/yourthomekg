using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class Facilities
    {
        public int FacilitiesID { get; set; }
        public int RentalID { get; set; }
        [JsonIgnore]
        public Rental Rental { get; set; }
        public bool Internet { get; set; }
        public bool Phone { get; set; }
        public bool Refrigerator { get; set; }
        public bool Kitchen { get; set; }
        public bool TV { get; set; }
        public bool Balcony { get; set; }
        public bool Washer { get; set; }
        public bool AirConditioning { get; set; }
    }
}
