using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Notices.Pages
{
    public class EditModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public EditModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WLC.Models.Notices Notices { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notices = await _context.Notices
                .Include(n => n.NoticeStatus)
                .Include(n => n.NoticeType).FirstOrDefaultAsync(m => m.NoticeId == id);

            if (Notices == null)
            {
                return NotFound();
            }
           ViewData["NoticeStatusId"] = new SelectList(_context.NoticeStatuses, "NoticeStatusId", "Description");
           ViewData["NoticeTypeId"] = new SelectList(_context.NoticeTypes, "NoticeTypeId", "Description");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Notices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticesExists(Notices.NoticeId))
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

        private bool NoticesExists(int id)
        {
            return _context.Notices.Any(e => e.NoticeId == id);
        }
    }
}
