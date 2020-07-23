using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class UserSpare
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        //public Avatar Avatar { get; set; }
        //public List<Rental> Rentals { get; set; }
    }


}