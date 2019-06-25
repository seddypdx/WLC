using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class RibbonsInStock
    {
        public int RibonId { get; set; }
        public string Ribbon { get; set; }
        public int? NumberInStock { get; set; }
    }
}
