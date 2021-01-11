using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WLC.Models
{
    public partial class Notices
    {
        public Notices()
        {
            NoticeQueueItems = new HashSet<NoticeQueueItems>();
        }

        public int NoticeId { get; set; }

        [Display(Name = "Notice Type")]
        public int NoticeTypeId { get; set; }

        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateToSend { get; set; }
    
        [Display(Name = "Notice Status")]
        public int NoticeStatusId { get; set; }

        public NoticeStatuses NoticeStatus { get; set; }
        public NoticeTypes NoticeType { get; set; }
        public ICollection<NoticeQueueItems> NoticeQueueItems { get; set; }
    }
}
