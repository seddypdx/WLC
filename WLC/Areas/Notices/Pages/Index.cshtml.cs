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
    public class IndexModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public IndexModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        public IList<WLC.Models.Notices> Notices { get;set; }

        public async Task OnGetAsync()
        {
            Notices = await _context.Notices
                .Include(n => n.NoticeStatus)
                .Include(n => n.NoticeType).ToListAsync();
        }
    }
}
