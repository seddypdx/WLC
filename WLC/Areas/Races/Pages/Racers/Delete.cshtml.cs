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
    public class DeleteModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public DeleteModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Racers = await _context.Racers.FindAsync(id);

            if (Racers != null)
            {
                _context.Racers.Remove(Racers);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
