using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class Races
    {
        public Races()
        {
            Results = new HashSet<Results>();
        }

        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public double? Year { get; set; }
        public string RacePoints { get; set; }
        public double? Participants { get; set; }
        public string RaceBoyOrGirl { get; set; }
        public double? MinimumAge { get; set; }
        public double? MaximumAge { get; set; }
        public double? SortOrder { get; set; }
        public int? RaceOrder { get; set; }
        public bool IsBoating { get; set; }

        public virtual ICollection<Results> Results { get; set; }
    }
}
