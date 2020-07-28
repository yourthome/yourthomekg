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
        public string AvatarName { get; set; }
        public string AvatarPath { get; set; }
    }
}
