using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class FacFilter
    {
        public bool Internet { get; set; } = false;
        public bool Phone { get; set; } = false;
        public bool Refrigerator { get; set; } = false;
        public bool Kitchen { get; set; } = false;
        public bool TV { get; set; } = false;
        public bool Balcony { get; set; } = false;
        public bool Washer { get; set; } = false;
        public bool AirConditioning { get; set; } = false;
    }
}
