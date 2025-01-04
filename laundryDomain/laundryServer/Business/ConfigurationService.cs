using laundryHeart.Domain.Entities;
using laundryServer.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace laundryServer.Business
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationDAO _configurationDAO;

        public ConfigurationService(IConfigurationDAO configurationDAO)
        {
            _configurationDAO = configurationDAO;
        }

        public Dictionary<string, object> GetConfiguration()
        {
            return _configurationDAO.GetConfiguration();
        }
    }
}
