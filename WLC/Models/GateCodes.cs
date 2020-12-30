using System;
using System.Collections.Generic;

namespace WLC.Models
{
    public partial class GateCodes
    {
        public int GatecodeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Code { get; set; }
        public int? CabinId { get; set; }
    }
}
