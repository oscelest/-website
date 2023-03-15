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
        public string Subject { get; set; }
        public DateTime IssueDate { get; set; }

        private static readonly JwtSecurityTokenHandler Handler = new();

        public JWT(Guid id, string issuer, string audience, string subject)
        {
            UserID = id;
            Issuer = issuer;
            Audience = audience;
            Subject = subject;
            IssueDate = DateTime.Now;
            TokenID = Guid.NewGuid();
        }

        public JWT(JwtSecurityToken token)
        {
            UserID = Guid.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value, out Guid NameID) ? NameID : Guid.Empty;
            TokenID = Guid.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value, out Guid JTI) ? JTI : Guid.Empty;
            Issuer = token.Issuer;
            Subject = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;
            Audience = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value ?? string.Empty;
            IssueDate = DateTime.TryParse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat)?.Value, out DateTime IAT) ? IAT : DateTime.MinValue;
        }

        public string Sign(string secret, DateTime? activation = null, DateTime? expiration = null)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
            SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);
            DateTime activates = activation ?? DateTime.UtcNow;
            DateTime expires = expiration ?? activates.AddDays(7);

            Claim[] claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, UserID.ToString()),
            };

            return Handler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, activates, expires, signIn));
        }

        public static JWT? ReadToken(string? token)
        {
            return Handler.CanReadToken(token) ? new JWT(Handler.ReadJwtToken(token)) : null;
        }
    }
}
