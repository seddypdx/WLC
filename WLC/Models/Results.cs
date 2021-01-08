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
        public int? TeamId { get; set; }
        public int? Place { get; set; }
        public int? PointsPlace { get; set; }
        public double? Points { get; set; }
        public string Ribbon { get; set; }
        public string Comments { get; set; }

        public Races Race { get; set; }
        public Racers Racer { get; set; }
        public Years YearNavigation { get; set; }
    }
}
