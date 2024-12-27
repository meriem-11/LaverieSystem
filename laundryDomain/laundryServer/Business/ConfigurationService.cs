using laundryHeart.Domain.Entities;
using laundryServer.Business.Interfaces;

namespace laundryServer.Business
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationDAO _configuration;

        public ConfigurationService(IConfigurationDAO configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Proprietaire>> GetConfigurationAsync()
        {

            return _configuration.Configuration;
        }
    }
}
