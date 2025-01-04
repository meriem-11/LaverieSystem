using laundryHeart.Domain.Entities;

namespace laundryServer.Business.Interfaces
{
    public interface IMachineService
    {
        List<Machine> GetMachines();  // Récupérer toutes les machines
        Machine GetMachineById(int id);  // Récupérer une machine par ID
        void UpdateMachineStatus(int id, bool isAvailable);  // 
    }
}
