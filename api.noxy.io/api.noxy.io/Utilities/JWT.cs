﻿using api.noxy.io.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.noxy.io.Utility
{
    public interface IJWT
    {
        public Guid GetUserID(ClaimsPrincipal claim);
        public Guid GetUserID(JwtSecurityToken token);
        public Guid GetUserID(ClaimsIdentity identity);
        public string Generate(UserEntity user, DateTime? activation = null, DateTime? expiration = null);
    }

    public class JWT : IJWT
    {
        private static readonly JwtSecurityTokenHandler Handler = new();
        private static readonly string NameID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        private readonly IConfiguration _config;

        public JWT(IConfiguration config)
        {
            _config = config;
        }

        public Guid GetUserID(ClaimsPrincipal claim)
        {
            if (claim?.Identity is ClaimsIdentity identity)
            {
                if (Guid.TryParse(identity.Claims.FirstOrDefault(x => x.Type == NameID)?.Value, out Guid id))
                {
                    return id;
                }
            }
            throw new SecurityTokenException();
        }

        public Guid GetUserID(JwtSecurityToken token)
        {
            return Guid.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value, out Guid id) 
                ? id 
                : throw new Exception("");
        }

        public Guid GetUserID(ClaimsIdentity identity)
        {
            return Guid.TryParse(identity.Claims.FirstOrDefault(x => x.Type == NameID)?.Value, out Guid id) 
                ? id 
                : throw new Exception("");
        }

        public string Generate(UserEntity user, DateTime? activation = null, DateTime? expiration = null)
        {
            string issuer = _config["JWT:Issuer"]!;
            string audience = _config["JWT:Audience"]!;
            string secret = _config["JWT:Secret"]!;

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);
            DateTime activates = activation ?? DateTime.UtcNow;
            DateTime expires = expiration ?? activates.AddDays(7);

            Claim[] claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
            };

            return Handler.WriteToken(new JwtSecurityToken(issuer, audience, claims, activates, expires, signIn));
        }

        public static JwtSecurityToken? ReadToken(string? token)
        {
            return Handler.CanReadToken(token) ? Handler.ReadJwtToken(token) : null;
        }
    }
}
