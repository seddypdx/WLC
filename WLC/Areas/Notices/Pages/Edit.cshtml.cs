using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WLC.Areas.Notices.Services;
using WLC.Models;

namespace WLC.Areas.Notices.Pages
{
    public class EditModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;
        private IConfiguration _config;
        private IEmailSender _emailSender;

        public EditModel(WLC.Models.WLCRacesContext context, IEmailSender emailSender, IConfiguration config)
        {
            _config = config;
            _context = context;
            _emailSender = emailSender;
        }

        [BindProperty]
        public WLC.Models.Notices Notices { get; set; }

        [BindProperty]
        public bool Resend { get; set; }

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

               // if (Resend)
                {
                    // for now just process on save
                    NotificationService.QueueNotification(_context, Notices);

                    NotificationService.ProcessNotificationQueue(_emailSender, _context);

                }

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

        private void SendTestEmail()
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("donotreply@waunalake.com");
            mail.To.Add("scotte@cascadecs.com");

            //set the content 
            mail.Subject = "This is an email";
            mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
            
            //send the message 
            SmtpClient smtp = new SmtpClient("m04.internetmailserver.net");

            smtp.UseDefaultCredentials = false;
            NetworkCredential Credentials = new NetworkCredential("donotreply@waunalake.com", "1W@unaL@ke3mailP@ssword");
            smtp.Credentials = Credentials;
           
            smtp.Send(mail);
           
        }
    }
}
