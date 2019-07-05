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
    public class IndexModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public IndexModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;
        }

        public IList<WLC.Models.Races> Races { get;set; }

        public async Task OnGetAsync()
        {
            Races = await _context.Races.ToListAsync();
        }
    }
}
