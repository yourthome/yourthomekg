using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourthome.Models.ViewModel
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public bool IsVerified { get; set; }
        public string AvatarName { get; set; }
        public string AvatarPath { get; set; }
    }
}
