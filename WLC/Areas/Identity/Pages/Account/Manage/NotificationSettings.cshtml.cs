using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WLC.Areas.Notices.Services;
using WLC.Models;

namespace WLC.Areas.Identity.Pages.Account.Manage
{
    public class NotificationSettingsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly WLCRacesContext _context;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;


        [BindProperty]
        public Member LoggedOnMemeber { get; set; }

        [TempData]
       public string StatusMessage { get; set; }


        public NotificationSettingsModel(
            WLCRacesContext context,
            UserManager<IdentityUser> userManager,
            ILogger<PersonalDataModel> logger,
            SignInManager<IdentityUser> signInManager,
        IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LoggedOnMemeber =  await GetMemberFromUser(user);
    
           if (LoggedOnMemeber == null)
                return NotFound($"Unable to load member from user  with Email '{user.Email}'.");


            return Page();
        }

        private async Task<Member> GetMemberFromUser(IdentityUser user)
        {
            var member = _context.Members.Where(x => x.AspNetUserId == user.Id).FirstOrDefault();

            if (member != null)
                return member;

            // try by email
            member = _context.Members.Where(x => x.EmailName == user.Email).FirstOrDefault();

            if (member != null)
                return member;

            return null;

  
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var member = await GetMemberFromUser(user);

            member.HomeCell = LoggedOnMemeber.HomeCell;
            member.NotifyOnEmergencyEmail = LoggedOnMemeber.NotifyOnEmergencyEmail;
            member.NotifyOnEmergencyText = LoggedOnMemeber.NotifyOnEmergencyText;
            member.NotifyOnInformationEmail = LoggedOnMemeber.NotifyOnInformationEmail;
            member.NotifyOnInformationText = LoggedOnMemeber.NotifyOnInformationText;
            member.NotifyOnSocialEmail = LoggedOnMemeber.NotifyOnSocialEmail;
            member.NotifyOnSocialText = LoggedOnMemeber.NotifyOnSocialText;
            member.NotifyOnEmergencyEmail = LoggedOnMemeber.NotifyOnEmergencyEmail;

            _context.Update(member);
            _context.SaveChanges();

  
            StatusMessage = "Your Notification settings have been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendTestEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var member = await GetMemberFromUser(user);

               await _emailSender.SendEmailAsync(
                member.EmailName ,
                "Testing email",
                $"I hope this email reached you safely. ");


            StatusMessage = "Test Email Sent";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendTestTextAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var member = await GetMemberFromUser(user);
            var email = NotificationService.GetMemeberTextEmail(member);

            await _emailSender.SendEmailAsync(
               email,
               "",
               $"Text Coming at you ");


            StatusMessage = "Test Text Sent";
            return RedirectToPage();
        }
    }
}