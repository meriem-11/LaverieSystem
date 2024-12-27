using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundryHeart.Domain.Entities
{
    public class Cycle
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
