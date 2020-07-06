using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }


        public string Password { get; set; }
    
    }

    public enum Gender
    {

        [Display(Name = "Мужской")]
        Male,

        [Display(Name = "Женский")]
        Female,

    }
}
