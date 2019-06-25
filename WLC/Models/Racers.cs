using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class Racers
    {
        public Racers()
        {
            Results = new HashSet<Results>();
        }

        public int RacerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BoyOrGirl { get; set; }
        public double? Age { get; set; }
        public DateTime? Birthdate { get; set; }
        public int MemberStatusId { get; set; }
        public int CabinId { get; set; }

        public virtual Cabins Cabin { get; set; }
        public virtual MemberStatuses MemberStatus { get; set; }
        public virtual ICollection<Results> Results { get; set; }
    }
}
