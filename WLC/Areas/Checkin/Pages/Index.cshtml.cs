using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Checkin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly WLC.Models.WLCRacesContext _context;

        public IndexModel(WLC.Models.WLCRacesContext context)
        {
            _context = context;

        }

        [BindProperty]
        public WLC.Models.Checkin Checkin { get; set; }

        public bool Success { get; set; }

        public SelectList CabinList;
        public SelectList MemberList;

        public void InitializeLookups()
        {
            CabinList = new SelectList(_context.Cabins.ToList().OrderBy(x => x.CabinName), "CabinId", "CabinName");

            if (Checkin != null && Checkin.CabinId >0 )
            {
                MemberList = new SelectList(_context.Members.Where(x => x.CabinId == Checkin.CabinId && x.Deceased == false &&
                                             (x.MemberTypeId == 2 || x.MemberTypeId == 4 || x.MemberTypeId == 12 || x.MemberTypeId == 15 || x.MemberTypeId == 21)).ToList().OrderBy(x => x.FirstName), "MemberId", "FullName");
            }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            // see if the user has been here before
            var memberId = GetMemberFromCookie();
            if (memberId > 0)
            {
                FindLastCheckin(memberId);
            }
            

            if (Checkin == null)
            {
                Checkin = new Models.Checkin()
                {
                    CheckInTime = DateTime.Now
                };
            }

            InitializeLookups();


            return Page();
        }


        private void FindLastCheckin(int memberId)
        {
            var lastCheckin = _context.Checkins.OrderByDescending(x => x.CheckinId)
                                        .FirstOrDefault(x => x.MemberId == memberId);
            var member = _context.Members.FirstOrDefault(x => x.MemberId == memberId);

            if (member == null)
            {
                return;
            }

            if (Checkin != null && member.CabinId != Checkin.CabinId && Checkin.CabinId > 0)
            {
                return;
            }

            if (lastCheckin != null)
            {
                // need to set the cabin too
                if (member != null && member.CabinId != null)
                    lastCheckin.CabinId = member.CabinId;

                if (lastCheckin.CheckOutTime == null)
                {
                    Checkin = lastCheckin;

                }
                else
                    Checkin = new Models.Checkin()
                    {
                        CabinId = lastCheckin.CabinId,
                        MemberId = lastCheckin.MemberId,
                        CheckInTime = DateTime.Now
                    };

            }
        }

        public async Task<IActionResult> OnPostAsync(string CheckinButton, string CheckoutButton)
        {
            if (string.IsNullOrEmpty(CheckinButton) && string.IsNullOrEmpty(CheckoutButton))
            {
                ModelState.Clear();

                if (Checkin.MemberId > 0 && Checkin.CheckinId == 0)
                {
                    FindLastCheckin(Checkin.MemberId);
                }

                InitializeLookups();
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(Checkin.CheckinId == 0)
             {
                Checkin.CheckInTime = DateTime.Now;
                _context.Checkins.Add(Checkin);
            }
            else
            {
                Checkin.CheckOutTime = DateTime.Now;
                _context.Attach(Checkin).State = EntityState.Modified;

            }

           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckinExists(Checkin.CheckinId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                InitializeLookups();
                return Page();
            }

            Success = true;

            SaveCookieForReturn();

            return Page();
        }

        private void SaveCookieForReturn()
        {
            try
            {
                if (Checkin.MemberId == 0)
                    return;

                CookieOptions option = new CookieOptions();

                option.Expires = DateTime.Now.AddDays(60);

                Response.Cookies.Append("MemberId", Checkin.MemberId.ToString(), option);

            }
            catch (Exception ex)
            {

            }
        }

        private int GetMemberFromCookie()
        {
            var x = Request.Cookies["MemberId"];

            if (string.IsNullOrEmpty(x))
                return 0;

            int memberId = 0;

            if (int.TryParse(x, out memberId))
                return memberId;

            return 0;
        }

        private bool CheckinExists(int id)
        {
            return _context.Checkins.Any(e => e.CheckinId == id);
        }
    }
}
