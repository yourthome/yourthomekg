﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yourthome.Data;
using Yourthome.Helpers;
using Yourthome.Models;
using Yourthome.Models.ViewModel;
using Yourthome.Services;
using Yourthome.ViewModel;

namespace Yourthome.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonalPageController : ControllerBase
    {
        private readonly YourthomeContext _context;
        private IUserService _userService;
        private IMapper _mapper;
        public PersonalPageController(YourthomeContext context, IUserService userService,
            IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet("User/{id}")]
        public async Task<ActionResult<User>> User(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        [HttpPut("User/{id}/edit")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm]UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;
            if (user.Avatar != null)
            {
                byte[] ImageData = null;
                using (var binaryReader = new BinaryReader(user.Avatar.OpenReadStream()))
                {
                    ImageData = binaryReader.ReadBytes((int)user.Avatar.Length);
                }
                // установка массива байтов
                user.AvatarStored = ImageData;
            }
            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("User/{id}/rentals")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUserRentals(int id)
        {
            var rentals = _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).Include(r => r.Photos).
                Include(r => r.Bookings).AsQueryable();
            rentals = _context.Rental.Where(p => p.UserID == id);
            return await rentals.ToListAsync();
        }
        [HttpGet("User/{id}/rentals/id")]
        public async Task<ActionResult<Rental>> GetUserRental(int id)
        {
            var rental = await _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).Include(r => r.Photos).
                Include(r => r.Bookings)
                .SingleOrDefaultAsync(r => r.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }
            return rental;
        }
        [HttpPost("User/{id}/rentals/create")]
        public async Task<ActionResult<Rental>> PostRental(int userid, [FromForm]RentalViewModel rvm)
        {
            for (int i = 0; i < 10; i++)
            {
                rvm.Bookings.Add(new Booking() { GuestName = $"Guest{i}" });
            }
            Rental rental = new Rental
            {
                UserID = userid, //or connect like user = context.user.get(userid) and add include to user
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
                Photos = new List<Photo>()
            };
            if (rvm.Photos != null)
            {
                foreach (var img in rvm.Photos)
                {
                    byte[] ImageData = null;
                    using (var binaryReader = new BinaryReader(img.OpenReadStream()))
                    {
                        ImageData = binaryReader.ReadBytes((int)img.Length);
                    }
                    // установка массива байтов
                    rental.Photos.Add(new Photo { Image = ImageData });
                }
            }
            _context.Rental.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalID }, rental);
        }
        [HttpPut("User/{id}/rentals/edit")]
        public async Task<IActionResult> UpdateRental(int id, Rental rental)
        {
            if (id != rental.RentalID)
            {
                return BadRequest();
            }
            _context.Entry(rental).State = EntityState.Modified;
            _context.Entry(rental.Facilities).State = EntityState.Modified;
            _context.Entry(rental.Infrastructure).State = EntityState.Modified;
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
        [HttpDelete("User/{id}/rentals/delete")]
        public async Task<ActionResult<Rental>> DeleteRental(int ID)
        {
            var rental = await _context.Rental.FindAsync(ID);
            if (rental == null)
            {
                return NotFound();
            }

            _context.Rental.Remove(rental);
            await _context.SaveChangesAsync();
            return rental;
        }
        private bool RentalExists(int ID)
        {
            return _context.Rental.Any(e => e.RentalID == ID);
        }
    }
}