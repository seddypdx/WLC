using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Races.Pages.Events
{
    public class DeleteModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public DeleteModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WLC.Models.Races Races { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Races = await _context.Races.FirstOrDefaultAsync(m => m.RaceId == id);

            if (Races == null)
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

            Races = await _context.Races.FindAsync(id);

            if (Races != null)
            {
                _context.Races.Remove(Races);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
