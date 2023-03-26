using api.noxy.io.Interface;
using api.noxy.io.Models;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace api.noxy.io.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionController : ControllerBase
    {
        private readonly IJWT _jwt;
        private readonly IUserRepository _user;
        private readonly IGameRepository _game;

        public MissionController(IJWT jwt, IUserRepository user, IGameRepository game)
        {
            _jwt = jwt;
            _user = user;
            _game = game;
        }

        [HttpPost("Start")]
        public async Task<ActionResult<GuildEntity.DTO?>> Start()
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            GuildEntity? guild = await _game.FindByUser(user);
            return Ok(guild?.ToDTO());
        }

        [HttpPost("Register")]
        public async Task<ActionResult<GuildEntity.DTO>> Register(GuildRegisterRequest input)
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            GuildEntity? guild = await _game.FindByUser(user);
            if (guild != null) return Conflict();

            guild = await _game.Create(input.Name, user);

            return Ok(guild.ToDTO());
        }

        [HttpPost("RefreshRecruitment")]
        public async Task<ActionResult<GuildEntity.DTO>> RefreshRecruitment()
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            return Ok((await _game.RefreshUnitList(user)).ToDTO());
        }


        public class GuildRegisterRequest
        {
            [Required(AllowEmptyStrings = false)]
            [StringLength(64, MinimumLength = 3)]
            public string Name { get; set; } = string.Empty;
        }
    }
}
