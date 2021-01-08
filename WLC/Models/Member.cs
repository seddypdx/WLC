using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WLC.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int CabinId { get; set; }
        public int? PrimaryMemberId { get; set; }
        public int? SecondaryMemberId { get; set; }
        public int? Spouse { get; set; }
        public int? MemberTypeId { get; set; }
        public bool Deceased { get; set; }
        public string HomeCell { get; set; }

        public string AspNetUserId { get; set; }

        public bool NotifyOnEmergency { get; set; }
        public bool NotifyOnInformation { get; set; }
        public bool NotifyOnSocial { get; set; }

        public Cabins Cabin { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }

        }


    }
}
