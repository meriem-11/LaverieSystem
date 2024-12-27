using laundryHeart.Domain.Entities;

namespace laundryServer.Business.Interfaces
{
    public interface IConfigurationDAO
    {
        public List<Proprietaire> Configuration { get; }
    }
}
