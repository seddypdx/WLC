using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Races.Pages.Racers
{
    public class EditModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public EditModel(WLC.Models.WLCRacesContext context)
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
           ViewData["CabinId"] = new SelectList(_context.Cabins, "CabinId", "CabinId");
           ViewData["MemberStatusId"] = new SelectList(_context.MemberStatuses, "MemberStatusId", "MemberStatus");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Racers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RacersExists(Racers.RacerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RacersExists(int id)
        {
            return _context.Racers.Any(e => e.RacerId == id);
        }
    }
}
