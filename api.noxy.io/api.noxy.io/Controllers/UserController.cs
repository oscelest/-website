using api.noxy.io.Interface;
using api.noxy.io.Models;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserController(IConfiguration Configuration, IUserRepository UserRepository)
        {
            _configuration = Configuration;
            _userRepository = UserRepository;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<UserEntity.DTO>> SignUp(UserAuthRequest input)
        {
            try
            {
                UserEntity user = await _userRepository.Create(input.Email, input.Password);
                return Ok(new UserEntity.DTO(user, GenerateToken(user)));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is MySqlException mysqlex)
                {
                    if (mysqlex.Number == 1062) return Conflict();
                }
            }
            catch { 
                // We should log the exception here.
            }
            
            return Problem();
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserEntity.DTO>> LogIn(UserAuthRequest input)
        {
            try
            {
                UserEntity? user = await _userRepository.FindByEmail(input.Email);
                if (user == null) return Unauthorized();
                byte[] hash = UserEntity.GenerateHash(input.Password, user.Salt);
                return hash.SequenceEqual(user.Hash) ? Ok(new UserEntity.DTO(user, GenerateToken(user))) : Unauthorized();
            }
            catch
            {
                return Problem();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Refresh")]
        public async Task<ActionResult<UserEntity.DTO>> Refresh()
        {
            try
            {
                var authorization = await HttpContext.GetTokenAsync("access_token");
                var token = JWT.ReadToken(authorization);
                if (token == null) return Unauthorized();

                UserEntity? user = await _userRepository.FindByID(token.UserID);
                if (user == null) return Unauthorized();
                return Ok(new UserEntity.DTO(user, GenerateToken(user)));
            }
            catch
            {
                return Problem();
            }
        }

        private string GenerateToken(UserEntity user)
        {
            return new JWT(user.ID, _configuration["JWT:Issuer"]!, _configuration["JWT:Audience"]!, _configuration["JWT:Subject"]!).Sign(_configuration["JWT:Secret"]!);
        }
    }

    public class UserAuthRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(254, MinimumLength = 6)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(12)]
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;
    }


}
