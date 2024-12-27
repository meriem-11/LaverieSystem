using laundryHeart.Domain.Entities;

namespace laundryServer.Business.Interfaces
{
    public interface IConfigurationService
    {
        public Task<List<Proprietaire>> GetConfigurationAsync();

    }
}
