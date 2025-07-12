using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace WebAPI.Configurations.AWS
{
    public class ParameterStoreService : IParameterStoreService
    {
        private readonly IAmazonSimpleSystemsManagement _ssmClient;

        public ParameterStoreService(IAmazonSimpleSystemsManagement ssmClient)
        {
            _ssmClient = ssmClient;
        }

        public async Task<string> GetParameterAsync(string name)
        {
            var request = new GetParameterRequest
            {
                Name = name,
                WithDecryption = true
            };

            var response = await _ssmClient.GetParameterAsync(request);
            return response.Parameter?.Value ?? string.Empty;
        }
    }
}
