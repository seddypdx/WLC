using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Races.Pages.Racers
{
    public class IndexModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public IndexModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        public IList<WLC.Models.Racers> Racers { get;set; }

        public async Task OnGetAsync()
        {
            Racers = await _context.Racers
                .Include(r => r.Cabin)
                .Include(r => r.MemberStatus).ToListAsync();
        }
    }
}
