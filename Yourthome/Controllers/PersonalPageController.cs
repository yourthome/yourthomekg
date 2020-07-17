using System;
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
        [HttpGet("User/{id}/rentals")]
        public async Task<ActionResult<IEnumerable<Rental>>> UserRentals(int id)
        {
            var rentals = _context.Rental.AsQueryable();
            rentals = _context.Rental.Where(p => p.UserID == id);
            return await rentals.ToListAsync();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm]UpdateModel model)
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
    }
}