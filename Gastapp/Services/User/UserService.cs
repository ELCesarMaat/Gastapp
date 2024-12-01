using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gastapp.Models;
using Refit;

namespace Gastapp.Services.User
{
    public class UserService : IUserService
    {
        readonly IUserService _userService;

        public UserService()
        {
            _userService = RestService.For<IUserService>(Constants.URL);
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _userService.LoginAsync(email, password);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginResponse();
            }

        }

        public async Task<LoginResponse> RefreshTokenAsync(string token)
        {
            try
            {
                var response = await _userService.RefreshTokenAsync(token);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginResponse();
            }

        }
    }
}
