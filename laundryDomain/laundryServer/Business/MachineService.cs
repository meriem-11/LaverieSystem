using laundryHeart.Domain.Entities;
using laundryServer.Business.Interfaces;
using System.Collections.Generic;

namespace laundryServer.Business
{
    public class MachineService : IMachineService
    {
        private readonly IMachineDAO _machineDAO;

        // Injection de dépendance de IMachineDAO
        public MachineService(IMachineDAO machineDAO)
        {
            _machineDAO = machineDAO;
        }

        // Récupérer toutes les machines
        public List<Machine> GetMachines()
        {
            return _machineDAO.GetMachines();  // Appel au DAO pour récupérer les machines
        }

        // Récupérer une machine par son ID
        public Machine GetMachineById(int id)
        {
            return _machineDAO.GetMachineById(id);  // Appel au DAO pour récupérer une machine par ID
        }

        // Mettre à jour le statut d'une machine
        public void UpdateMachineStatus(int id, bool isAvailable)
        {
            _machineDAO.UpdateMachineStatus(id, isAvailable);  // Mise à jour du statut via le DAO
        }
    }
}
