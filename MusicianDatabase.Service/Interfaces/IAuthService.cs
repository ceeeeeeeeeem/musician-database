using Microsoft.AspNetCore.Identity;
using MusicianDatabase.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateUser(string email, string password);
        Task<User> RegisterUser(string name, string surname, string email, string password);
        Task<bool> UserAlreadyExists(string email);

        string GenerateJwtToken(User user);
    }

}
