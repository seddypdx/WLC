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
    public class DetailsModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public DetailsModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        public WLC.Models.Racers Racers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Racers = await _context.Racers
                .Include(r => r.Cabin)
                .Include(r => r.MemberStatus).FirstOrDefaultAsync(m => m.RacerId == id);

            if (Racers == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
