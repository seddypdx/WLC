using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WLC.Models
{
    public partial class Racers
    {
        public Racers()
        {
            Results = new HashSet<Results>();
        }

        public int RacerId { get; set; }

        [Display(Name = "First Name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Display(Name = "Boy Or Girl")]
        [StringLength(1)]
        public string BoyOrGirl { get; set; }
       // public int Age { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Status")]
        public int MemberStatusId { get; set; }

        [Display(Name = "Cabin")]
        public int CabinId { get; set; }

        public virtual Cabins Cabin { get; set; }
        public virtual MemberStatuses MemberStatus { get; set; }
        public virtual ICollection<Results> Results { get; set; }

        //internal void SetAge()
        //{
        //    var today = DateTime.Today;
        //    // Calculate the age.
        //    Age = today.Year - Birthdate.GetValueOrDefault().Year;
        //    // Go back to the year the person was born in case of a leap year
        //    if (Birthdate.GetValueOrDefault().Date > today.AddYears(-Age)) Age--;
        //}

        public int GetAge(DateTime asOfDate)
        {
            var age = asOfDate.Year - Birthdate.GetValueOrDefault().Year;
            // Go back to the year the person was born in case of a leap year
            if (Birthdate.GetValueOrDefault().Date > asOfDate.AddYears(-age)) age--;

            return age;
        }
    }
}
