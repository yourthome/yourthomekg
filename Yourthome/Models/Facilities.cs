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
        public bool? Internet { get; set; } = true;
        public bool? Phone { get; set; } = true;
        public bool? Refrigerator { get; set; } = true;
        public bool? Kitchen { get; set; } = true;
        public bool? TV { get; set; } = true;
        public bool? Balcony { get; set; } = true;
        public bool? Washer { get; set; } = true;
        public bool? AirConditioning { get; set; } = true;
    }
}
