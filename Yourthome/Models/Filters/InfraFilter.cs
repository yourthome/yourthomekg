using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class InfraFilter
    {
        public bool Cafe { get; set; } = false;
        public bool Kindergarten { get; set; } = false;
        public bool Parking { get; set; } = false;
        public bool BusStop { get; set; } = false;
        public bool Supermarket { get; set; } = false;
        public bool Park { get; set; } = false;
        public bool Hospital { get; set; } = false;
    }
}
