using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WLC.Services;

namespace WLC.Areas.Notices.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int ActiveYear { get; set; }

        public void OnGet()
        {
            ActiveYear = Globals.GetActiveYear(HttpContext);
        }

        public void OnPost()
        {
            Globals.SetActiveYear(HttpContext, ActiveYear);
        }

    }
}