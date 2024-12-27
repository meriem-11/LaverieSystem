using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundryHeart.Domain.Entities
{
    public class Proprietaire
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Laverie> Laveries { get; set; } = new List<Laverie>();
    }
}
