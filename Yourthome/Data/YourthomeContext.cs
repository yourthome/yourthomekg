using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yourthome.Data;
using Yourthome.Models;

namespace Yourthome.Data
{
    public class YourthomeContext : DbContext
    {
        public YourthomeContext(DbContextOptions<YourthomeContext> options)
            : base(options)
        {
        }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Facilities> Facilities { get; set; }
        public DbSet<Infrastructure> Infrastructure { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
