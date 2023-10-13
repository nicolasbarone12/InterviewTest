using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Auth
{
    public class JwtTokenService
    {
        private readonly IConfiguration _config;
        public long DURATION_SECONDS = 600;

        public JwtTokenService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateJwtToken(string pass, string IdUsuario/*, string audience*/)
        {
            var jwtSettings = _config.GetSection(OAuthConstants.JWT_SETTINGS);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings[OAuthConstants.SECRET_KEY]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = "";// jwtSettings[OAuthConstants.VALID_ISSUER];

            // Genera y devuelve el token JWT
            var jwtToken = JwtAuthentication.GenerateJwtToken(credentials, pass, IdUsuario, issuer, /*audience,*/ DURATION_SECONDS);
            return jwtToken;
        }
    }
}
