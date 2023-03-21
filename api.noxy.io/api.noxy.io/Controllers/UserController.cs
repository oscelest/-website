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
using System.IdentityModel.Tokens.Jwt;

namespace api.noxy.io.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IJWT _jwt;
        private readonly IUserRepository _user;

        public UserController(IJWT jwt, IUserRepository user, IConfiguration config)
        {
            _jwt = jwt;
            _user = user;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<UserEntity.DTO>> SignUp(UserAuthRequest input)
        {
            try
            {
                UserEntity user = await _user.Create(input.Email, input.Password);
                return Ok(user.ToDTO(_jwt.Generate(user)));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is MySqlException mysqlex)
                {
                    if (mysqlex.Number == 1062) return Conflict();
                }
            }
            catch
            {
                // We should log the exception here.
            }

            return Problem();
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserEntity.DTO>> LogIn(UserAuthRequest input)
        {
            try
            {
                UserEntity? user = await _user.FindByEmail(input.Email);
                if (user == null) return Unauthorized();
                byte[] hash = UserEntity.GenerateHash(input.Password, user.Salt);
                return hash.SequenceEqual(user.Hash) ? Ok(user.ToDTO(_jwt.Generate(user))) : Unauthorized();
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
                string? authorization = await HttpContext.GetTokenAsync("access_token");
                JwtSecurityToken? token = JWT.ReadToken(authorization);
                if (token == null) return Unauthorized();

                UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
                if (user == null) return Unauthorized();
                return Ok(user.ToDTO(_jwt.Generate(user)));
            }
            catch
            {
                return Problem();
            }
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
