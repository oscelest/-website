using api.noxy.io.Interface;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("Load")]
        public async Task<ActionResult<IEnumerable<MissionEntity.DTO>>> Load()
        {
            List<UnitEntity> list = await _game.LoadUnitList(_jwt.GetUserID(User));
            return Ok(list.Select(x => x.ToDTO()));
        }

        [HttpPost("Initiate")]
        public async Task<ActionResult<MissionEntity.DTO>> Initiate(InitiateRequest input)
        {
            MissionEntity entity = await _game.InitiateMission(_jwt.GetUserID(User), input.MissionID, input.UnitIDList);
            return Ok(entity.ToDTO());
        }
        
        [HttpPost("RefreshAvailable")]
        public async Task<ActionResult<MissionEntity.DTO>> RefreshAvailable()
        {
            List<MissionEntity> list = await _game.RefreshAvailableMissionList(_jwt.GetUserID(User));
            return Ok(list.Select(x => x.ToDTO()));
        }

        public class InitiateRequest
        {
            public required List<Guid> UnitIDList { get; set; }
            public required Guid MissionID { get; set; }
        }
    }
}
