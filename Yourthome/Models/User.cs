using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Yourthome.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        [JsonIgnore]
        [NotMapped]
        public IFormFile Avatar { get; set; }
        public string AvatarName { get; set; }
        public string AvatarPath { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string VerificationToken { get; set; }
        public bool IsVerified { get; set; }
    }
    public enum Gender
    {
        [Display(Name = "Мужской")]
        Male,

        [Display(Name = "Женский")]
        Female
    }
}
