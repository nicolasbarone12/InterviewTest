using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Auth
{
    public class JwtAuthentication
    {
        public static void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection jwtSettings = configuration.GetSection(OAuthConstants.JWT_SETTINGS);
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings[OAuthConstants.SECRET_KEY]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = creds.Key,
                    ClockSkew = TimeSpan.FromSeconds(1),
                };
            });
        }

        public static string GenerateJwtToken(SigningCredentials signingCredentials, string pass, string userCode,
            string issuer, /*string audience,*/ long duration_seconds)
        {
            Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, pass), //Subject
                    new Claim("UserID", userCode), //Subject
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JSON Web Token ID
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) //IssuedAt
                };

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: issuer,
                    //audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddSeconds(duration_seconds),
                    signingCredentials: signingCredentials
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
