using laundryHeart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundryHeart.Domain.Interfaces
{
    internal interface IMachine
    {
        List<Machine> GetAllMachines();
        Machine GetMachineById(int id);
        void UpdateMachine(Machine machine);
    }
}
