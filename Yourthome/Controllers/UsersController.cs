using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Yourthome.Helpers;
using Yourthome.Models;
using Yourthome.Models.ViewModel;
using Yourthome.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Yourthome.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IIdsaferService _idsaferservice;
        IWebHostEnvironment _appEnvironment;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IIdsaferService idsaferservice,
            IWebHostEnvironment appEnvironment)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _idsaferservice = idsaferservice;
            _appEnvironment = appEnvironment;
        }
        /// <summary>
        /// User Log In
        /// </summary>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect. M.b. email isn't verified" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            _idsaferservice.TakeUserID(user.Id);
            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Token = tokenString
            });
        }
        /// <summary>
        /// User Register
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);
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
                // create user
                await _userService.Create(user,model.Password,Request.Headers["origin"]);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Email Verification
        /// </summary>
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            await _userService.VerifyEmail(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }
        
    }
}

