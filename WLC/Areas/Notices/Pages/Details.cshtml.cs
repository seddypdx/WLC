using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Notices.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public DetailsModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        public WLC.Models.Notices Notices { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notices = await _context.Notices
                .Include(n => n.NoticeStatus)
                .Include(n => n.NoticeQueueItems).ThenInclude(x => x.NoticeStatus)
                .Include(n => n.NoticeType).FirstOrDefaultAsync(m => m.NoticeId == id);


            if (Notices == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
