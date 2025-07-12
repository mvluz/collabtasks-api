namespace WebAPI.Configurations.Security.JWT
{
    public interface IJwtConfigurationService
    {
        void ConfigureJwt(WebApplicationBuilder builder);
    }

}
