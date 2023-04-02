using api.noxy.io.Interface;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Mvc;

namespace api.noxy.io.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnitController : ControllerBase
    {
        private readonly IJWT _jwt;
        private readonly IUserRepository _user;
        private readonly IGameRepository _game;

        public UnitController(IJWT jwt, IUserRepository user, IGameRepository guild)
        {
            _jwt = jwt;
            _user = user;
            _game = guild;
        }

        [HttpGet("Load")]
        public async Task<ActionResult<IEnumerable<UnitEntity.DTO>>> Load()
        {
            List<UnitEntity> list = await _game.LoadUnitList(_jwt.GetUserID(User));
            return Ok(list.Select(x => x.ToDTO()));
        }

        [HttpPost("Initiate")]
        public async Task<ActionResult<UnitEntity.DTO>> Initiate(InitiateRequest input)
        {
            UnitEntity unit = await _game.InitiateUnit(_jwt.GetUserID(User), input.UnitID);
            return Ok(unit.ToDTO());
        }

        [HttpPost("RefreshAvailable")]
        public async Task<ActionResult<UnitEntity.DTO>> RefreshAvailable()
        {
            List<UnitEntity> list = await _game.RefreshAvailableUnitList(_jwt.GetUserID(User));
            return Ok(list.Select(x => x.ToDTO()));
        }

        public class InitiateRequest
        {
            public Guid UnitID { get; set; } = Guid.Empty;
        }
    }
}
