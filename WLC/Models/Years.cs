using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class Years
    {
        public Years()
        {
            Results = new HashSet<Results>();
        }

        public int Year { get; set; }
        public DateTime? LaborDayDate { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Results> Results { get; set; }
    }
}
