using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yourthome.Models;
using Microsoft.AspNetCore.Http;

namespace Yourthome.ViewModel
{
    public class RentalViewModel
    {
        public Region Region { get; set; }
        public string Street { get; set; }
        public int Rooms { get; set; }
        public int Cost { get; set; }
        public PropertyType PropertyType { get; set; }
        public RentTime RentTime { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Facilities Facilities { get; set; }
        public Infrastructure Infrastructure { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<Booking> Bookings { get; set; }
        public RentalViewModel()
        {
            Bookings = new List<Booking> { };
        }
        
    }
    
}
