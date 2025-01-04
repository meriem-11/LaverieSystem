using laundryHeart.Domain.Entities;

namespace laundryServer.Business.Interfaces
{
    public interface IConfigurationDAO
    {
        Dictionary<string, object> GetConfiguration();

    }
}
