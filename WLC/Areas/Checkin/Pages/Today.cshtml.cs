using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WLC.Areas.Checkin.Pages
{
    public class TodayModel : PageModel
    {
        public IList<WLC.Models.Checkin> OnSiteList { get; set; }
        private readonly WLC.Models.WLCRacesContext _context;

        public TodayModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;

        }


        public void OnGet()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            OnSiteList = _context.Checkins
                                 .Where(x => x.CheckInTime > startDate)
                                 .Include(x => x.Member)
                                 .ToList();
                                 


        }
    }
}