using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public enum MemberStatusEnum
    {
        Member = 2,
        AdultMember = 3,
        Retiree = 4,
        Exhibition = 5,
        Guest = 6,
        AdultGuest = 7,
        Inactive = 8

    }

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
