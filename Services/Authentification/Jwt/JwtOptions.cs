using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ZeraMessanger.Services.Authentification.Jwt
{
    public record JwtOptions
    {
        public JwtOptions()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string secretKey = configuration.GetSection("SecretKey").ToString()!;

            SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public SymmetricSecurityKey SecurityKey { get; }

        public SigningCredentials SigningCredentials { get; }
    }
}
