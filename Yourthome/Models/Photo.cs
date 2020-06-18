using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public int  RentalID { get; set; }
        public Rental Rental { get; set; }
        public byte[] Image { get; set; }
    }
}
