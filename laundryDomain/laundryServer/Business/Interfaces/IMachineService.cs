using System.Reflection.PortableExecutable;

namespace laundryServer.Business.Interfaces
{
    public interface IMachineService
    {
        List<Machine> GetMachines();
        Machine GetMachineById(int id);
        void UpdateMachineStatus(int id, bool isAvailable);
    }
}
