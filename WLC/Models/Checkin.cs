﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WLC.Models
{
    public class Checkin
    {
        public int CheckinId { get; set; }

        [NotMapped]
        public int CabinId { get; set; }

        public int MemberId { get; set; }
        public string Note { get; set;  }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

    }
}