using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class Cabins
    {
        public Cabins()
        {
            Racers = new HashSet<Racers>();
        }

        public int CabinId { get; set; }
        public string CabinName { get; set; }
        public string CabinPhone { get; set; }

        public ICollection<Racers> Racers { get; set; }
    }
}
