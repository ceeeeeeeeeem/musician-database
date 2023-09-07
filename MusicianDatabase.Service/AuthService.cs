using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service
{
    public class AuthService : IAuthService
    {
        private readonly MusicianDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(MusicianDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            // Find the user in the database by their email
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

            // Check if a user with the given email exists
            if (user == null)
                _logger.LogInformation("User not found");
                return null; // User not found

            // Verify the provided password by hashing it with the stored salt
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null; // Password doesn't match

            // Authentication successful
            _logger.LogInformation("User authenticated.");

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        _logger.LogInformation("Password does not match.");

                    return false; // Password doesn't match
                }
            }
            _logger.LogInformation("Password verified.");

            return true; // Password matches
        }


        public string GenerateJwtToken(User user)
        {
            // Create claims that will be included in the JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                // You can add additional claims here based on your application's requirements
             };

            // Fetch your JWT secret key from configuration (keep it secret!)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT token with relevant settings
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: creds
            );

            // Serialize the JWT token to a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> RegisterUser(string name, string surname, string email, string password)
        {
            // Generate a random salt
            byte[] passwordSalt = GenerateSalt();

            // Hash the password with the generated salt
            byte[] passwordHash = HashPassword(password, passwordSalt);

            User user = new User();
            user.Name = name;
            user.Surname = surname;
            user.Email = email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation("Successfully registered {name} {surname} with email: {email}", name, surname, email);

                return user;
            }

            // if fails
            _logger.LogInformation("Could not create user.");

            return null;
        }

        private byte[] GenerateSalt()
        {
            // Create a random salt
            byte[] salt = new byte[128 / 8]; // 128 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        public async Task<bool> UserAlreadyExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }

}
