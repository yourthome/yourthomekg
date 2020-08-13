using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yourthome.Data;
using Yourthome.Helpers;
using Yourthome.Models;
using Yourthome.Models.ViewModel;
using Yourthome.Services;

namespace Yourthome.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly YourthomeContext _context;
        private IUserService _userService;
        private IMapper _mapper;
        IWebHostEnvironment _appEnvironment;
        public AdminController(YourthomeContext context, IUserService userService,
            IMapper mapper, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet("users")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }
        /// <summary>
        /// get specific user by id
        /// </summary>
        [HttpGet("users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }            
        /// <summary>
        /// delete user
        /// </summary>
        [HttpDelete("users/{id}/delete")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
        /// <summary>
        /// get rentals
        /// </summary>
        [HttpGet("rentals")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRental([FromQuery]Region? region)
        {
            var rents = _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).
                Include(r => r.Bookings).Include(r => r.Photos).
                AsQueryable();
            if (region.HasValue)
            {
                rents = rents.Where(r => r.Region == region); //filter by Region
            }
            return await rents.ToListAsync();
        }
        /// <summary>
        /// get rental by id
        /// </summary>
        [HttpGet("rentals/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).
                Include(r => r.Bookings).Include(r => r.Photos)
                .SingleOrDefaultAsync(r => r.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }
            return rental;
        }       
        /// <summary>
        /// delete rental
        /// </summary>
        [HttpDelete("rentals/{id}/delete")]
        [Authorize(Roles = Role.Admin)]
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
        
    }
}