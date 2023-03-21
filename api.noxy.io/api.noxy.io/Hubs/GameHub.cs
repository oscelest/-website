using api.noxy.io.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace api.noxy.io.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IGuildRepository _guildRepository;

        public GameHub(IUserRepository userRepository, IGuildRepository guildRepository)
        {
            _userRepository = userRepository;
            _guildRepository = guildRepository;
        }
    }
}
