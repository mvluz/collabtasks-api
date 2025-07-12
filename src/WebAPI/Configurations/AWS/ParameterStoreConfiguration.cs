namespace WebAPI.Configurations.AWS
{
    public class ParameterStoreConfiguration
    {
        private readonly IParameterStoreService _parameterStoreService;
        private readonly ILogger<ParameterStoreConfiguration> _logger;

        public ParameterStoreConfiguration(IParameterStoreService parameterStoreService, ILogger<ParameterStoreConfiguration> logger)
        {
            _parameterStoreService = parameterStoreService;
            _logger = logger;
        }

        public async Task ConfigureAppSettingsAsync(IConfiguration configuration)
        {
            try
            {
                // List of expected parameters
                var requiredParameters = new Dictionary<string, string>
            {
                { "WriteConnectionString", "/auth/db/write-connection-string" },
                { "ReadConnectionString", "/auth/db/read-connection-string" },
                { "JwtKey", "/auth/security/jwt-secret-key" },
                { "JwtIssuer", "/auth/security/jwt-issuer" },
                { "JwtAudience", "/auth/security/jwt-audience" },
                { "JwtExpirationTime", "/auth/security/jwt-expiration-time" }
            };

                // Get parameter values
                var parameterValues = new Dictionary<string, string>();

                foreach (var param in requiredParameters)
                {
                    var value = await _parameterStoreService.GetParameterAsync(param.Value);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        _logger.LogError($"Required parameter '{param.Key}' is missing or empty.");
                        throw new Exception($"Required parameter '{param.Key}' is missing or empty.");
                    }

                    parameterValues[param.Key] = value;
                }

                // Set the values in app.Configuration
                configuration["ConnectionStrings:WriteConnection"] = parameterValues["WriteConnectionString"];
                configuration["ConnectionStrings:ReadConnection"] = parameterValues["ReadConnectionString"];
                configuration["Jwt:Key"] = parameterValues["JwtKey"];
                configuration["Jwt:Issuer"] = parameterValues["JwtIssuer"];
                configuration["Jwt:Audience"] = parameterValues["JwtAudience"];
                configuration["Jwt:ExpirationTime"] = parameterValues["JwtExpirationTime"];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading parameters from Parameter Store.");
                throw;
            }
        }
    }
}
