using api.noxy.io.Interface;
using api.noxy.io.Models.Auth;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuildController : ControllerBase
    {
        private readonly IJWT _jwt;
        private readonly IUserRepository _user;
        private readonly IGameRepository _game;

        public GuildController(IJWT jwt, IUserRepository user, IGameRepository game)
        {
            _jwt = jwt;
            _user = user;
            _game = game;
        }

        [HttpGet("Load")]
        public async Task<ActionResult<GuildEntity.DTO?>> Load()
        {
            GuildEntity? guild = await _game.LoadGuild(_jwt.GetUserID(User));
            return Ok(guild?.ToDTO());
        }

        [HttpPost("Register")]
        public async Task<ActionResult<GuildEntity.DTO>> Register(RegisterRequest input)
        {
            UserEntity user = await _user.FindOne(_jwt.GetUserID(User));
            GuildEntity guild = await _game.CreateGuild(user, input.Name);
            return Ok(guild.ToDTO());
        }

        public class RegisterRequest
        {
            [Required(AllowEmptyStrings = false)]
            [StringLength(64, MinimumLength = 3)]
            public string Name { get; set; } = string.Empty;
        }
    }
}
