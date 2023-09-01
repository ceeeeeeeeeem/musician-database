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
        Task<User> AuthenticateUserAsync(string email, string password);
        string GenerateJwtToken(User user);
    }

}
