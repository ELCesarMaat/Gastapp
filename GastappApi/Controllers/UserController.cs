using GastappApi.DTOs;
using GastappApi.Models;
using GastappApi.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GastappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GastappDbContext _dbContext;
        private readonly UserUtils _userUtils;

        public UserController(GastappDbContext dbContext, UserUtils userUtils)
        {
            _dbContext = dbContext;
            _userUtils = userUtils;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult<ResponseMessage>> RegisterUser(UserRegisterDTO user)
        {
            if (await _dbContext.Users.AnyAsync(p => p.Email == user.Email))
                return BadRequest(ResponseMessages.EmailAlreadyRegistered);

            if (await _dbContext.Users.AnyAsync(p => p.Username == user.Username))
                return BadRequest(ResponseMessages.UsernameAlreadyRegistered);

            if (!PasswordUtils.IsPasswordSafe(user.Password))
                return BadRequest(ResponseMessages.PasswordNotSafe);

            string passwordSalt = PasswordUtils.GenerateSalt();
            string passwordHash = PasswordUtils.HashPassword(user.Password, passwordSalt);

            var newUser = new User
            {
                Email = user.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = user.PhoneNumber,
                Username = user.Username
            };

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();


            await _dbContext.SpendingCategories.AddAsync(new SpendingCategory
            {
                UserId = newUser.UserId,
                CategoryName = "Sin Nombre",
                IsInitial = true,
            });

            await _dbContext.SaveChangesAsync();
            return Ok(ResponseMessages.UserRegisteredSuccessfully);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseMessage>> LoginUser(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return BadRequest(ResponseMessages.UserNotFound);

            var passwordHash = PasswordUtils.HashPassword(password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash)
                return BadRequest(ResponseMessages.IncorrectPassword);

            var token = _userUtils.GenerateToken(user);

            return Ok(new ResponseMessage
            {
                Code = ResponseMessages.LoginSuccesfully.Code,
                Message = ResponseMessages.LoginSuccesfully.Message,
                Data = new UserLoginInfo()
                {
                    Token = token,
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email
                }
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<ResponseMessage>> RefreshToken(string token)
        {
            var principal = _userUtils.GetPrincipalFromToken(token);

            if (principal == null)
                return BadRequest("Invalid token");

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                return BadRequest("User not found");

            var newToken = _userUtils.GenerateToken(user);

            UserLoginInfo userResponse = new UserLoginInfo
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                Token = newToken
            };

            return Ok(new ResponseMessage
            {
                Code = ResponseMessages.TokenRefreshed.Code,
                Message = ResponseMessages.TokenRefreshed.Message,
                Data = userResponse
            });
        }
    }
}