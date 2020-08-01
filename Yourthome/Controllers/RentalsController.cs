using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yourthome.Data;
using Yourthome.Models;
using Yourthome.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Yourthome.Services;
using Microsoft.AspNetCore.Hosting;

namespace Yourthome.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly YourthomeContext _context;
        IWebHostEnvironment _appEnvironment;
        private IIdsaferService _idsaferservice;
        public RentalsController(YourthomeContext context,IIdsaferService idsaferservice, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _idsaferservice = idsaferservice;
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// Find all rentals
        /// </summary>
        // GET: Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRental([FromQuery]Region? region, [FromQuery]int? rooms,
            [FromQuery]PropertyType? property, [FromQuery]RentTime? renttime,
            [FromQuery]int? CostrangeStart, [FromQuery] int? CostrangeEnd, [FromQuery]FacFilter facfilter,
            [FromQuery]InfraFilter infrafilter, [FromQuery]Sort? sort)
        {
            var rents = _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).
                Include(r => r.Bookings).Include(r=>r.Photos).
                AsQueryable();
            if (region.HasValue)
            {
                rents = rents.Where(r => r.Region == region); //filter by Region
            }
            if (rooms.HasValue)
            {
                rents = rents.Where(r => r.Rooms == rooms); //filter by amount of room
            }
            if (property.HasValue)
            {
                rents = rents.Where(r => r.PropertyType == property); //filter by property type (house or apartment)
            }
            if (renttime.HasValue)
            {
                rents = rents.Where(r => r.RentTime == renttime); //filter by rent time 
            }
            if(CostrangeStart.HasValue && CostrangeEnd.HasValue)
            {
                rents = rents.Where(r => r.Cost>=CostrangeStart && r.Cost<=CostrangeEnd); // filter in cost range
            }
            if (facfilter.Internet != null && facfilter.AirConditioning != null)
            {
                rents = rents.Where(r => r.Facilities.AirConditioning == facfilter.AirConditioning && r.Facilities.Internet == facfilter.Internet
                && r.Facilities.Phone == facfilter.Phone && r.Facilities.Washer == facfilter.Washer
                && r.Facilities.Refrigerator == facfilter.Refrigerator && r.Facilities.Balcony == facfilter.Balcony
                && r.Facilities.Kitchen == facfilter.Kitchen && r.Facilities.TV == facfilter.TV
                && r.Facilities.Internet == facfilter.Internet);
            }
            if (infrafilter.Cafe != null && infrafilter.Hospital != null)
            {
                rents = rents.Where(r => r.Infrastructure.BusStop == infrafilter.BusStop && r.Infrastructure.Parking == infrafilter.Parking
                && r.Infrastructure.Kindergarten == infrafilter.Kindergarten && r.Infrastructure.Cafe == infrafilter.Cafe
                && r.Infrastructure.Supermarket == infrafilter.Supermarket && r.Infrastructure.Park == infrafilter.Park
                && r.Infrastructure.Hospital == infrafilter.Hospital);
            }                    
            if (sort.HasValue)
            {
                switch(sort)
                {
                    case Sort.ASC:
                        rents = rents.OrderBy(r => r.Cost); //sort by cost in ascending order
                        break;
                    case Sort.DESC:
                        rents = rents.OrderByDescending(r => r.Cost); //sort by cost in descending order
                        break;
                }
            }
            return await rents.ToListAsync();
        }

        /// <summary>
        /// Find specific rental by ID
        /// </summary>
        // GET: Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).
                Include(r=>r.Bookings).Include(r => r.Photos)
                .SingleOrDefaultAsync(r => r.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }
            return rental;
        }
        /// <summary>
        /// Edits specific rental by ID
        /// </summary>
        // PUT: Rentals/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> PutRental(int id, RentalViewModel rvm)
        {
            var rental = await _context.Rental.FindAsync(id);
            if (id != rental.RentalID)
            {
                return BadRequest();
            }
            rental.Region = rvm.Region;
            rental.Street = rvm.Street;
            rental.Rooms = rvm.Rooms;
            rental.Cost = rvm.Cost;
            rental.PropertyType = rvm.PropertyType;
            rental.RentTime = rvm.RentTime;
            rental.Description = rvm.Description;
            rental.Latitude = rvm.Latitude;
            rental.Longitude = rvm.Longitude;
            rental.Facilities = rvm.Facilities;
            rental.Infrastructure = rvm.Infrastructure;
            rental.Bookings = rvm.Bookings;
            rental.Photos = new List<ImageModel>();
            foreach (var uploadedFile in rvm.Photos)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                ImageModel file = new ImageModel { Name = uploadedFile.FileName, Path = path };
                rental.Photos.Add(file);
            }
            _context.Entry(rental).State = EntityState.Modified;
            _context.Entry(rental.Facilities).State = EntityState.Modified;
            _context.Entry(rental.Infrastructure).State = EntityState.Modified;
            foreach (var i in rental.Photos)
            {
                _context.Entry(i).State = EntityState.Modified;
            }
            foreach (var i in rental.Bookings)
            {
                _context.Entry(i).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        /// <summary>
        /// Create new rental
        /// </summary>
        // POST: Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD. 
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Rental>> PostRental([FromForm]RentalViewModel rvm)
        {
            for (int i = 0; i < 10; i++)
            {
                rvm.Bookings.Add(new Booking() { GuestName = $"Guest{i}" });
            }
            Rental rental = new Rental
            {
                UserID = _idsaferservice.GetUserID(), //or connect like user = context.user.get(userid) and add include to user
                Region = rvm.Region,
                Street = rvm.Street,
                Rooms = rvm.Rooms,
                Cost = rvm.Cost,
                PropertyType = rvm.PropertyType,
                RentTime = rvm.RentTime,
                Description = rvm.Description,
                Latitude = rvm.Latitude,
                Longitude = rvm.Longitude,
                Facilities = rvm.Facilities,
                Infrastructure = rvm.Infrastructure,
                Bookings = rvm.Bookings,
                Photos = new List<ImageModel> { }
            };
            if (rvm.Photos != null)
            {
                foreach (var uploadedFile in rvm.Photos)
                {
                    // путь к папке Files
                    string path = "/Files/" + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    ImageModel file = new ImageModel { Name = uploadedFile.FileName, Path = path };
                    rental.Photos.Add(file);
                }
            }
            
            _context.Rental.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalID }, rental);
        }
        /// <summary>
        /// Delete specific rental by ID
        /// </summary>
        // DELETE: Rentals/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Rental>> DeleteRental(int id)
        {
            var rental = await _context.Rental.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            _context.Rental.Remove(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        private bool RentalExists(int id)
        {
            return _context.Rental.Any(e => e.RentalID == id);
        }
    }
}

