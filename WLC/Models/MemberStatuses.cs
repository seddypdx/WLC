using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class MemberStatuses
    {
        public MemberStatuses()
        {
            Racers = new HashSet<Racers>();
        }

        public int MemberStatusId { get; set; }
        public string MemberStatus { get; set; }

        public virtual ICollection<Racers> Racers { get; set; }
    }
}
