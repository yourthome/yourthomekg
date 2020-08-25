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
        private readonly ICloudStorage _cloudStorage;
        public RentalsController(YourthomeContext context,IIdsaferService idsaferservice,
            ICloudStorage cloudStorage, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _idsaferservice = idsaferservice;
            _appEnvironment = appEnvironment;
            _cloudStorage = cloudStorage;
        }
        /// <summary>
        /// Find all rentals
        /// </summary>
        // GET: Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRental([FromQuery]Region? region, [FromQuery]int? rooms,
            [FromQuery]int? floor,[FromQuery]string title,
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
            if (floor.HasValue)
            {
                rents = rents.Where(r => r.Floor == floor); //filter by floor
            }
            if(!string.IsNullOrWhiteSpace(title))
            {
                rents = rents.Where(r => r.Title.Contains(title));
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
            //--------------------------------Facilities Filter--------------------------------
            if (facfilter.Internet == true)
            {
                rents = rents.Where(r => r.Facilities.Internet == facfilter.Internet);
            }
            if (facfilter.Phone == true)
            {
                rents = rents.Where(r => r.Facilities.Phone == facfilter.Phone);
            }
            if (facfilter.Refrigerator == true)
            {
                rents = rents.Where(r => r.Facilities.Refrigerator == facfilter.Refrigerator);
            }
            if (facfilter.Kitchen == true)
            {
                rents = rents.Where(r => r.Facilities.Kitchen == facfilter.Kitchen);
            }
            if (facfilter.Balcony == true)
            {
                rents = rents.Where(r => r.Facilities.Balcony == facfilter.Balcony);
            }
            if (facfilter.Washer == true)
            {
                rents = rents.Where(r => r.Facilities.Washer == facfilter.Washer);
            }
            if (facfilter.AirConditioning == true)
            {
                rents = rents.Where(r => r.Facilities.AirConditioning == facfilter.AirConditioning);
            }
            if (facfilter.Internet == true)
            {
                rents = rents.Where(r => r.Facilities.Internet == facfilter.Internet);
            }
            //--------------------------------Infrastructure Filter--------------------------------
            if (infrafilter.Cafe == true)
            {
                rents = rents.Where(r => r.Infrastructure.Cafe == infrafilter.Cafe);
            }
            if (infrafilter.Kindergarten == true)
            {
                rents = rents.Where(r => r.Infrastructure.Kindergarten == infrafilter.Kindergarten);
            }
            if (infrafilter.Parking == true)
            {
                rents = rents.Where(r => r.Infrastructure.Parking == infrafilter.Parking);
            }
            if (infrafilter.BusStop == true)
            {
                rents = rents.Where(r => r.Infrastructure.BusStop == infrafilter.BusStop);
            }
            if (infrafilter.Supermarket == true)
            {
                rents = rents.Where(r => r.Infrastructure.Supermarket == infrafilter.Supermarket);
            }
            if (infrafilter.Park == true)
            {
                rents = rents.Where(r => r.Infrastructure.Park == infrafilter.Park);
            }
            if (infrafilter.Hospital == true)
            {
                rents = rents.Where(r => r.Infrastructure.Hospital == infrafilter.Hospital);
            }          
            if (sort.HasValue)   //sort by cost order
            {
                switch(sort)
                {
                    case Sort.ASC:
                        rents = rents.OrderBy(r => r.Cost); 
                        break;
                    case Sort.DESC:
                        rents = rents.OrderByDescending(r => r.Cost); 
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
                UserID = _idsaferservice.GetUserID(), 
                Title = rvm.Title,
                Region = rvm.Region,
                Street = rvm.Street,
                Rooms = rvm.Rooms,
                Cost = rvm.Cost,
                Floor = rvm.Floor,
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
                    ImageModel file = new ImageModel { Name = uploadedFile.FileName};
                    file.Path = await _cloudStorage.UploadFileAsync(uploadedFile, file.Name);
                    rental.Photos.Add(file);
                }
            }          
            _context.Rental.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalID }, rental);
        }
        
        private bool RentalExists(int id)
        {
            return _context.Rental.Any(e => e.RentalID == id);
        }
    }
}

