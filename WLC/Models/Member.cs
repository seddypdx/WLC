using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        
        public string HomeCell { get; set; }

        public string AspNetUserId { get; set; }

        public string EmailName { get; set; }
        public bool NotifyOnEmergencyEmail { get; set; }
        public bool NotifyOnInformationEmail { get; set; }
        public bool NotifyOnSocialEmail { get; set; }
        public bool NotifyOnEmergencyText { get; set; }
        public bool NotifyOnInformationText { get; set; }
        public bool NotifyOnSocialText { get; set; }

        public Cabins Cabin { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }

        }

        public ICollection<Checkin> Checkins{ get; set; }

    }
}
