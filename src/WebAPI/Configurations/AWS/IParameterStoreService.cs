namespace WebAPI.Configurations.AWS
{
    public interface IParameterStoreService
    {
        Task<string> GetParameterAsync(string name);
    }
}
