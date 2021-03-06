﻿using System;
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
        public bool? Internet { get; set; } = null;
        public bool? Phone { get; set; } = null;
        public bool? Refrigerator { get; set; } = null;
        public bool? Kitchen { get; set; } = null;
        public bool? TV { get; set; } = null;
        public bool? Balcony { get; set; } = null;
        public bool? Washer { get; set; } = null;
        public bool? AirConditioning { get; set; } = null;
    }
}
