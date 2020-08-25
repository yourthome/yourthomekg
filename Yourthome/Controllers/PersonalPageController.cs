using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Authorize]
    public class PersonalPageController : ControllerBase
    {
        private readonly YourthomeContext _context;
        private IUserService _userService;
        private IMapper _mapper;
        private IIdsaferService _idsaferservice;
        IWebHostEnvironment _appEnvironment;
        private readonly ICloudStorage _cloudStorage;

        public PersonalPageController(YourthomeContext context, IUserService userService,
            IMapper mapper, IIdsaferService idsaferservice, 
            IWebHostEnvironment appEnvironment, ICloudStorage cloudStorage)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _idsaferservice = idsaferservice;
            _appEnvironment = appEnvironment;
            _cloudStorage = cloudStorage;
        }
        /// <summary>
        /// Get logged user's info
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<User>> User()
        {
            int id = _idsaferservice.GetUserID();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        /// <summary>
        /// edit logged user's info
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = _idsaferservice.GetUserID();
            if (user.Avatar != null)
            {
                // путь к папке Files
                string path = "/Files/" + user.Avatar.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await user.Avatar.CopyToAsync(fileStream);
                }
                ImageModel file = new ImageModel { Name = user.Avatar.FileName, Path = path };
                user.AvatarName = file.Name;
                user.AvatarPath = file.Path;
            }
            try
            {
                // update user 
                await _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Get rentals posted by logged user
        /// </summary>
        [HttpGet("getuserrentals")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUserRentals()
        {
            int id = _idsaferservice.GetUserID();
            var rentals = _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).
                Include(r => r.Bookings).Include(r=>r.Photos).AsQueryable();
            rentals = _context.Rental.Where(p => p.UserID == id);
            return await rentals.ToListAsync();
        }
        /// <summary>
        /// get specific rental posted by logged user
        /// </summary>
        [HttpGet("getuserrentals/{id}")]
        public async Task<ActionResult<Rental>> GetUserRentals(int id)
        {
            int userid = _idsaferservice.GetUserID();
            var rental = await _context.Rental.Include(r => r.Facilities).Include(r => r.Infrastructure).
               Include(r => r.Bookings).Include(r => r.Photos).Where(r=>r.UserID==userid)
               .SingleOrDefaultAsync(r => r.RentalID == id);
            if (rental == null)
            {
                return NotFound();
            }
            return rental;
        }
        /// <summary>
        /// post rental from user's page
        /// </summary>
        [HttpPost("postrental")]
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
                Floor = rvm.Floor,
                Title = rvm.Title,
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
                    ImageModel file = new ImageModel { Name = uploadedFile.FileName };
                    file.Path = await _cloudStorage.UploadFileAsync(uploadedFile, file.Name);
                    rental.Photos.Add(file);
                }
            }          
            await _context.Rental.AddAsync(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserRentals", new { id = rental.RentalID }, rental);
        }
        /// <summary>
        /// edit rental posted by user
        /// </summary>
        [HttpPut("updaterental/{id}")]
        public async Task<IActionResult> UpdateRental(int id, RentalViewModel rvm)
        {
            var rental = await _context.Rental.FindAsync(id);
            if (id != rental.RentalID)
            {
                return BadRequest();
            }
            rental.Region = rvm.Region;
            rental.Street = rvm.Street;
            rental.Title = rvm.Title;
            rental.Rooms = rvm.Rooms;
            rental.Cost = rvm.Cost;
            rental.Floor = rvm.Floor;
            rental.PropertyType = rvm.PropertyType;
            rental.RentTime = rvm.RentTime;
            rental.Description = rvm.Description;
            rental.Latitude = rvm.Latitude;
            rental.Longitude = rvm.Longitude;
            rental.Facilities = rvm.Facilities;
            rental.Infrastructure = rvm.Infrastructure;
            rental.Bookings = rvm.Bookings;
            rental.Photos = new List<ImageModel> { };
            if (rvm.Photos != null)
            {
                var img = _context.Photos.Where(r => r.RentalID == rental.RentalID);
                _context.Photos.RemoveRange(img);
                foreach (var uploadedFile in rvm.Photos)
                {
                    ImageModel file = new ImageModel { Name = uploadedFile.FileName };
                    file.Path = await _cloudStorage.UploadFileAsync(uploadedFile, file.Name);
                    rental.Photos.Add(file);
                }
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
        /// <summary>
        /// book a rental
        /// </summary>
        [HttpPut("Booking/{id}")]
        public async Task<IActionResult> Booking(int id, BookingViewModel book)
        {
            var rental = await _context.Rental.FindAsync(id);
            if (id != rental.RentalID)
            {
                return BadRequest();
            }          
            rental.Bookings = book.Bookings;
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
        /// delete user's rental
        /// </summary>
        [HttpDelete("deleterental/{id}")]
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