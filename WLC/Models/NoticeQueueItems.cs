using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class NoticeQueueItems
    {
        public int NoticeQueueItemId { get; set; }
        public int NoticeId { get; set; }
        public int MessageId { get; set; }
        public int NoticeStatusId { get; set; }
        public DateTime DateLastChanged { get; set; }
        public string NotificationLocation { get; set; }

        public Notices Notice { get; set; }
        public NoticeStatuses NoticeStatus { get; set; }
    }
}
