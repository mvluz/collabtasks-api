using Amazon.SimpleSystemsManagement;
using WebAPI.Configurations.AWS;
using WebAPI.Configurations.HealthCheck;
using WebAPI.Configurations.Logging;
using WebAPI.Configurations.Middleware;
using WebAPI.Configurations.Security.JWT;
using WebAPI.Configurations.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
SerilogConfiguration.ConfigureLogging(builder);

// Add AWS, Parameter Store, and other services
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleSystemsManagement>();
builder.Services.AddSingleton<IParameterStoreService, ParameterStoreService>();
builder.Services.AddSingleton<ParameterStoreConfiguration>();

// Database configuration
//var dbConfigService = new DatabaseConfigurationService(builder.Configuration);
//dbConfigService.ConfigureDbContexts(builder);

// JWT configuration
var jwtConfigService = new JwtConfigurationService(builder.Configuration);
jwtConfigService.ConfigureJwt(builder);

// Swagger configuration
SwaggerConfiguration.ConfigureSwaggerServices(builder.Services);

// Add Health Check
HealthCheckConfiguration.ConfigureHealthChecks(builder);

// Controller configuration
builder.Services.AddControllers();

var app = builder.Build();

// Perform health check during startup
// using (var scope = app.Services.CreateScope())
// {
//     var healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
//     var healthReport = await healthCheckService.CheckHealthAsync();

//     // Se algum health check falhar, a aplica��o ir� lan�ar uma exce��o e n�o iniciar
//     if (healthReport.Status != HealthStatus.Healthy)
//     {
//         throw new InvalidOperationException("A aplica��o n�o pode iniciar, um servi�o essencial n�o est� saud�vel.");
//     }
// }

// Get service instance
var parameterStoreConfig = app.Services.GetRequiredService<ParameterStoreConfiguration>();
await parameterStoreConfig.ConfigureAppSettingsAsync(app.Configuration);

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

SwaggerConfiguration.ConfigureSwaggerUI(app);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Map Health Check
app.MapHealthChecks("/health");

app.Run();