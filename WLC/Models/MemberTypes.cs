using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class MemberTypes
    {
        public int MemberTypeId { get; set; }
        public string MemberType { get; set; }
        public decimal? MemberDues { get; set; }
        public bool Active { get; set; }
        public string Category { get; set; }
        public bool MembershipType { get; set; }
        public bool Family { get; set; }
        public short? RosterSortOrder { get; set; }
        public short? RosterFamilySortOrder { get; set; }
    }
}
