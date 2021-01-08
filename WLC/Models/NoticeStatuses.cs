using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class NoticeStatuses
    {
        public NoticeStatuses()
        {
            NoticeQueueItems = new HashSet<NoticeQueueItems>();
            Notices = new HashSet<Notices>();
        }

        public int NoticeStatusId { get; set; }
        public string Description { get; set; }

        public ICollection<NoticeQueueItems> NoticeQueueItems { get; set; }
        public ICollection<Notices> Notices { get; set; }
    }
}
