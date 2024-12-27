using laundryHeart.Domain;
using laundryHeart.Domain.Entities;

namespace laundryServer.Business.Interfaces
{
    public interface ICycleService
    {
        List<Cycle> GetAllCycles();

    }
}
