using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WLC.Models;

namespace WLC.Areas.Races.Pages.Racers
{
    public class CreateModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public CreateModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CabinId"] = new SelectList(_context.Cabins, "CabinId", "CabinId");
        ViewData["MemberStatusId"] = new SelectList(_context.MemberStatuses, "MemberStatusId", "MemberStatus");
            return Page();
        }

        [BindProperty]
        public WLC.Models.Racers Racers { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Racers.Add(Racers);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}