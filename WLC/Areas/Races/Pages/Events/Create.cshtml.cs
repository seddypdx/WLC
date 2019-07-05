using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WLC.Models;

namespace WLC.Areas.Races.Pages.Events
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
            return Page();
        }

        [BindProperty]
        public WLC.Models.Races Races { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Races.Add(Races);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}