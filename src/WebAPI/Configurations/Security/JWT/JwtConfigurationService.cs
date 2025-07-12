using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Configurations.Security.JWT
{
    public class JwtConfigurationService : IJwtConfigurationService
    {
        private readonly IConfiguration _configuration;

        public JwtConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureJwt(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.FromSeconds(Convert.ToDouble(_configuration["Jwt:ExpirationTime"]))
                    };
                });
        }
    }
}
