using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gastapp.Models;
using Refit;

namespace Gastapp.Services.User
{
    public interface IUserService
    {
        [Post("/User/register")]
        Task<RegisterResponse> RegisterUserAsync(string email, string password, string name);


        [Post("/User/login")]
        Task<LoginResponse> LoginAsync([AliasAs("email")]string email, [AliasAs("password")]string password);


        [Post("/User/RefreshToken")]
        Task<LoginResponse> RefreshTokenAsync(string token);
    }
}
