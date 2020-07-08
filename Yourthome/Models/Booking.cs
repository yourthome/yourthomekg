﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int RentalID { get; set; }
        public Rental Rental { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime EvictionDate { get; set; }
        public string GuestName { get; set; }
    }
}
