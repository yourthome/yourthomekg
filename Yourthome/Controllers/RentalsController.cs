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

namespace Yourthome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly YourthomeContext _context;

        public RentalsController(YourthomeContext context)
        {
            _context = context;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRental([FromQuery]Region? region,[FromQuery]int? rooms,
            [FromQuery]PropertyType? property,[FromQuery]RentTime? renttime,
            [FromQuery]int? CostrangeStart,[FromQuery] int? CostrangeEnd,[FromQuery]Sort? sort)
        {
            var rents = _context.Rental.AsQueryable();
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


        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rental.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        // PUT: api/Rentals/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental(int id, Rental rental)
        {
            if (id != rental.RentalID)
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

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

        // POST: api/Rentals
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(RentalViewModel rvm)
        {
            Rental rental = new Rental {
                Region = rvm.Region,
                Street = rvm.Street,
                Rooms = rvm.Rooms,
                Cost = rvm.Cost,
                PropertyType = rvm.PropertyType,
                RentTime = rvm.RentTime,
                Furniture = rvm.Furniture,
                Nearby = rvm.Nearby,
                Description = rvm.Description,
                Latitude = rvm.Latitude,
                Longitude = rvm.Longitude
            };
            if(rvm.Photos!=null)
            {
                int count = 0;
                foreach (var img in rvm.Photos)
                {
                    byte[] ImageData = null;
                    using (var binaryReader = new BinaryReader(img.OpenReadStream()))
                    {
                        ImageData = binaryReader.ReadBytes((int)img.Length);
                    }
                    // установка массива байтов
                    rental.Photos[count].Image = ImageData;
                    count++;
                }              
            }
            _context.Rental.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalID }, rental);
        }

        // DELETE: api/Rentals/5
        [HttpDelete("{id}")]
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
