using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class Results
    {
        public int ResultId { get; set; }
        public int Year { get; set; }
        public int? RaceId { get; set; }
        public int? RacerId { get; set; }
        public double? TeamId { get; set; }
        public double? Place { get; set; }
        public double? PointsPlace { get; set; }
        public double? Points { get; set; }
        public string Ribbon { get; set; }
        public string Comments { get; set; }

        public virtual Races Race { get; set; }
        public virtual Racers Racer { get; set; }
        public virtual Years YearNavigation { get; set; }
    }
}
