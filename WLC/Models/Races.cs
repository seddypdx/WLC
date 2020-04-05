using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int Year { get; set; }
        public string RacePoints { get; set; }
        public int Participants { get; set; }
        public string RaceBoyOrGirl { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public int SortOrder { get; set; }
        public int RaceOrder { get; set; }
        public bool IsBoating { get; set; }

        [NotMapped]
        public double PointMultiplier {
            get {
                if (Participants > 1)
                    return .5;

                return 1;
           }
        }


        public string GetColorForPlace(int place)
       {
            if (RacePoints == "Participation Ribbon Only")
                return "Purple";


            if (place == 1)
                return "Blue";

            if (place == 2)
                return "Red";

            if (place == 3)
                return "White";

            return "Purple";

        }

        public double GetPointsForRibon(string ribon)
        {

            if (RacePoints == "Exhibition: 1 Point all places")
                return 1;

            if (ribon == "Blue")
                return 10 * PointMultiplier;
            if (ribon == "Red")
                return 7 * PointMultiplier;
            if (ribon == "White")
                return 4 * PointMultiplier;

            return 1;
        }



        public virtual ICollection<Results> Results { get; set; }
    }
}
