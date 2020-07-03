using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WLC.Areas.Checkin.Pages
{
    public class OnSiteModel : PageModel
    {
        public IList<WLC.Models.Checkin> OnSiteList { get; set; }
        private readonly WLC.Models.WLCRacesContext _context;

        public OnSiteModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;

        }


        public void OnGet()
        {
            OnSiteList = _context.Checkins
                                 .Where(x => x.CheckOutTime == null)
                                 .Include(x => x.Member).ThenInclude(y => y.Cabin)
                                 .ToList();
                                 


        }
    }
}