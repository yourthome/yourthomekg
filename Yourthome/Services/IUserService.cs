using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Yourthome.Data;
using Yourthome.Helpers;
using Yourthome.Models;

namespace Yourthome.Services
{
    public interface IUserService
    {
        void CreateAdmin();
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Create(User user, string password,string origin);
        Task VerifyEmail(string token);
        Task Update(User user, string password = null);
        Task Delete(int id);
    }
    public class UserService : IUserService
    {
        private YourthomeContext _context;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;
        public UserService(YourthomeContext context, IEmailService emailService,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _emailService = emailService;
            _appSettings = appSettings.Value;
            CreateAdmin();
        }
        public void CreateAdmin()
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == 1);
            if (user==null)
            {
                User admin = new User { FirstName = "Admin", LastName = "Adminov", Username = "adminbratan" };
                byte[] passwordHash, passwordSalt;
                string password = "neugadaew";
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                admin.PasswordHash = passwordHash;
                admin.PasswordSalt = passwordSalt;
                admin.Role = Role.Admin;
                admin.IsVerified = true;
                _context.Users.Add(admin);
                _context.SaveChanges();
            }           
        }
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            // check if username exists
            if (user == null)   
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            /*if (!user.IsVerified)
                return null;*/

            // authentication successful
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var rents = _context.Users.Where(u => u.Id != 1);
            return await rents.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> Create(User user, string password,string origin)
        {
            // validation           
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            if (_context.Users.Any(x => x.Email == user.Email))
            {
                // send already registered error in email to prevent account enumeration
                sendAlreadyRegisteredEmail(user.Email, origin);
                throw new AppException("Email \"" + user.Email + "\" is already used");
            }
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = Role.User;
            user.VerificationToken = randomTokenString();
            user.IsVerified = true;
            _context.Users.Add(user); 
            await _context.SaveChangesAsync();
            sendVerificationEmail(user, origin);
            return user;
        }
        public async Task VerifyEmail(string token)
        {
            var account =  await _context.Users.SingleOrDefaultAsync(x => x.VerificationToken == token);

            if (account == null) throw new AppException("Verification failed");
            account.IsVerified = true;
            account.VerificationToken = null;

            _context.Users.Update(account);
            await _context.SaveChangesAsync();
        }
        public async Task Update(User userParam, string password = null)
        {
            var user = await _context.Users.FindAsync(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
                user.Username = userParam.Username;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            if (!string.IsNullOrWhiteSpace(userParam.AvatarName))
                user.AvatarName = userParam.AvatarName;
            if (!string.IsNullOrWhiteSpace(userParam.AvatarPath))
                user.AvatarPath = userParam.AvatarPath;
            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {         
            var user = _context.Users.Find(id);
            if (user != null && user.Role!=Role.Admin)
            {
                var rentals = _context.Rental.Where(r => r.UserID == id);
                _context.Rental.RemoveRange(rentals);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        private async void sendVerificationEmail(User account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/Users/verify-email?token={account.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/Users/verify-email</code> api route:</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            await _emailService.Send(
                to: account.Email,
                subject: "Yourthome Email Verification!",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }

        private async void sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            await _emailService.Send(
                to: email,
                subject: "Yourthome Email Verification! " +
                "Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );
        }
        private string randomTokenString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
