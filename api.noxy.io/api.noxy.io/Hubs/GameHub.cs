using api.noxy.io.Interface;
using api.noxy.io.Models;
using api.noxy.io.Models.Game;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

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

        public async Task Load()
        {
            JWT token = new((ClaimsIdentity)Context.User!.Identity!);
            
            UserEntity? user = await _userRepository.FindByID(token.UserID);
            if (user == null)
            {
                throw new HubException("Unauthorized");
            }

            GuildEntity? guild = await _guildRepository.FindByUser(user);
            if (guild == null)
            {
                await Clients.Caller.SendAsync("Load", null);
            }
            else
            {
                await Clients.Caller.SendAsync("Load", guild.ToDTO());
            }
        }
    }
}