using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLC.Models
{
    public enum MemberStatusEnum
    {
        Member = 2,
        AdultMember = 3,
        Retiree = 4,
        Exhibition = 5,
        Guest = 6,
        AdultGuest = 7,
        Inactive = 8

    }

    public enum NoticeTypeEnum
    {
        Social = 10,
        Informational = 20,
        Emergency = 30
 
    }

    public enum NoticeStatusEnum
    {
        New = 10,
        Submitted= 20,
        InProcess = 30,
        Completed= 40,
        Error = 50

    }
}
