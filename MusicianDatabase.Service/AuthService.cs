using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service
{
    public class AuthService : IAuthService
    {
        private readonly MusicianDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(MusicianDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public async Task<User> Register(User user, string password)
        //{
        //    // Implement user registration logic here, including password hashing
        //    // Save the user to the database
        //    // Return the created user

        //}

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            // Find the user in the database by their email
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

            // Check if a user with the given email exists
            if (user == null)
                return null; // User not found

            // For this exercise, we're not using hashed passwords, so compare plaintext passwords
            if (user.Password != password)
                return null; // Password doesn't match

            // Authentication successful
            return user;
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

    }

}
