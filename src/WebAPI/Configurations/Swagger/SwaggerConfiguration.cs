using Microsoft.OpenApi.Models;

namespace WebAPI.Configurations.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Collab Tasks API v1",
                    Version = "v1",
                    Description = "API Collab Tasks v1",
                });

                // c.SwaggerDoc("v2", new OpenApiInfo
                // {
                //     Title = " {} API v2",
                //     Version = "v2",
                //     Description = "API de {} Versão 2",
                // });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter the JWT token in the field below (Bearer {your token})",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                c.DocInclusionPredicate((version, apiDescription) =>
                {
                    var versionParameter = apiDescription.RelativePath?.Split('/')[0];
                    return versionParameter == version;
                });
            });
        }

        public static void ConfigureSwaggerUI(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Collab Tasks API v1");
                    //c.SwaggerEndpoint("/swagger/v2/swagger.json", "{} API v2");
                    c.RoutePrefix = string.Empty;
                });
            }
        }
    }
}
