using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.noxy.io.Utility
{
    public class JWT
    {
        public Guid UserID { get; set; }
        public Guid TokenID { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime IssueDate { get; set; }

        private static readonly JwtSecurityTokenHandler Handler = new();
        private static readonly string NameID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public JWT(Guid id, string issuer, string audience)
        {
            UserID = id;
            Issuer = issuer;
            Audience = audience;
            IssueDate = DateTime.Now;
            TokenID = Guid.NewGuid();
        }

        public JWT(JwtSecurityToken token)
        {
            UserID = Guid.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value, out Guid NameID) ? NameID : Guid.Empty;
            TokenID = Guid.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value, out Guid JTI) ? JTI : Guid.Empty;
            Issuer = token.Issuer;
            Audience = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value ?? string.Empty;
            IssueDate = DateTime.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat)?.Value, out DateTime IAT) ? IAT : DateTime.MinValue;
        }

        public JWT(ClaimsIdentity identity)
        {
            UserID = Guid.TryParse(identity.Claims.FirstOrDefault(x => x.Type == NameID)?.Value, out Guid nameID) ? nameID : Guid.Empty;
            TokenID = Guid.TryParse(identity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value, out Guid jti) ? jti : Guid.Empty;
            Issuer = identity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss)?.Value ?? string.Empty;
            Audience = identity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value ?? string.Empty;
            IssueDate = DateTime.TryParse(identity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat)?.Value, out DateTime iat) ? iat : DateTime.MinValue;
        }

        public string Sign(string secret, DateTime? activation = null, DateTime? expiration = null)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);
            DateTime activates = activation ?? DateTime.UtcNow;
            DateTime expires = expiration ?? activates.AddDays(7);

            Claim[] claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
            };

            return Handler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, activates, expires, signIn));
        }

        public static JWT? ReadToken(string? token)
        {
            return Handler.CanReadToken(token) ? new JWT(Handler.ReadJwtToken(token)) : null;
        }
    }
}
