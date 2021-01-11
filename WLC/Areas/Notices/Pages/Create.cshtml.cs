using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using WLC.Areas.Notices.Services;
using WLC.Models;

namespace WLC.Areas.Notices.Pages
{
    public class CreateModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        private IConfiguration _config;
        private IEmailSender _emailSender;

        public CreateModel(WLC.Models.WLCRacesContext context, IEmailSender emailSender, IConfiguration config)
        {
            _config = config;
            _context = context;
            _emailSender = emailSender;
        }

        public IActionResult OnGet()
        {
        ViewData["NoticeStatusId"] = new SelectList(_context.NoticeStatuses, "NoticeStatusId", "Description");
        ViewData["NoticeTypeId"] = new SelectList(_context.NoticeTypes, "NoticeTypeId", "Description");

   
            return Page();
        }

        [BindProperty]
        public WLC.Models.Notices NewNotice { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewNotice.NoticeStatusId = (int) NoticeStatusEnum.New;
            NewNotice.DateCreated = DateTime.Now;
            NewNotice.DateToSend = DateTime.Now;


            _context.Notices.Add(NewNotice);
            await _context.SaveChangesAsync();

            // for now just process on save
            NotificationService.QueueNotification(_context, NewNotice);

            NotificationService.ProcessNotificationQueue(_emailSender, _context);



            return RedirectToPage("./Index");
        }


    }
}