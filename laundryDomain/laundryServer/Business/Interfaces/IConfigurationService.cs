using laundryHeart.Domain.Entities;

namespace laundryServer.Business.Interfaces
{
    public interface IConfigurationService
    {
        Dictionary<string, object> GetConfiguration();

    }
}
