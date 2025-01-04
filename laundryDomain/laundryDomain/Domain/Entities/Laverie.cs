using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace laundryHeart.Domain.Entities
{
    public class Laverie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Machine> Machines { get; set; }
        public Proprietaire Proprietaire { get; set; }
    }
}
