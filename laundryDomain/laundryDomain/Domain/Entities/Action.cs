using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundryHeart.Domain.Entities
{
    public class Action
    {
        public int Id { get; set; }
        public string date { get; set; }
        public Cycle Cycle { get; set; }

    }
}
