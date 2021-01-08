using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class NoticeTypes
    {
        public NoticeTypes()
        {
            Notices = new HashSet<Notices>();
        }

        public int NoticeTypeId { get; set; }
        public string Description { get; set; }

        public ICollection<Notices> Notices { get; set; }
    }
}
