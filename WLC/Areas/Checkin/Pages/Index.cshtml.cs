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
            InitializeLookups();

        }

        [BindProperty]
        public WLC.Models.Checkin Checkin { get; set; }

        public bool Success { get; set; }

        public SelectList CabinList;
        public SelectList MemberList;
        public int CabinId;
        public int MemberId;
        public IList<Member> AssociatedMembers;
        public bool ValidateCabin;

        public void InitializeLookups()
        {

            if (CabinId >0 )
            {
                MemberList = new SelectList(_context.Members.Where(x => x.CabinId == CabinId && x.Deceased == false &&
                                             (x.MemberTypeId == 2 || x.MemberTypeId == 4 || x.MemberTypeId == 12 || x.MemberTypeId == 15 || x.MemberTypeId == 21)).ToList().OrderBy(x => x.FirstName), "MemberId", "FullName");
            }



            if (MemberList == null || !MemberList.Any(x => x.Value == MemberId.ToString()))  // if cabin changed to 
                MemberId = 0;

            if (MemberId > 0)
            {
                AssociatedMembers = _context.Members.Where(x => (x.SecondaryMemberId == MemberId || x.MemberId == MemberId || ((x.MemberTypeId==3 || x.MemberTypeId==6) &&  x.PrimaryMemberId== MemberId) && x.Deceased==false)).ToList();
            }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            CabinId = GetCabinFromCookie();
            if (CabinId == 0) // no cabin set yet
            {
                return Redirect("~/Checkin/SetCabin");
            }

            // on any get start with blank checkin
           Checkin = new Models.Checkin()
                {
                    CheckInTime = DateTime.Now
                };


            // see if the user has been here before
            var memberId = GetMemberFromCookie();
            if (memberId > 0)
            {
                FindLastCheckin(memberId);
            }



            MemberId = Checkin.MemberId;
            InitializeLookups();


            return Page();
        }


        private void FindLastCheckin(int memberId)
        {
           var member = _context.Members.FirstOrDefault(x => x.MemberId == memberId);

            if (member == null)
            {
                return;
            }

            var lastCheckin = _context.Checkins.OrderByDescending(x => x.CheckinId)
                             .FirstOrDefault(x => x.MemberId == memberId);

     

            if (lastCheckin != null)
            {
  
                if (lastCheckin.CheckOutTime == null)
                {
                    Checkin = lastCheckin;

                }
                else
                    Checkin = new Models.Checkin()
                    {
                        MemberId = lastCheckin.MemberId,
                        CheckInTime = DateTime.Now
                    };

            }
        }

        public async Task<IActionResult> OnPostChangeMemberAsync(int memberId)
        {

            CabinId = GetCabinFromCookie();

            MemberId = memberId;
            ModelState.Clear();

            if (Checkin.MemberId > 0 && Checkin.CheckinId == 0)
            {
                FindLastCheckin(Checkin.MemberId);
            }
            Checkin.MemberId = MemberId;

                InitializeLookups();
                return Page();

          
        }

        public async Task<IActionResult> OnPostRecordCheckinAsync()
        {
     

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

            SetMemberCookie(Checkin.MemberId);

            return Page();
        }



        private void SetMemberCookie(int memberId)
        {
            try
            {
 
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(60);
                Response.Cookies.Append("MemberId", Checkin.MemberId.ToString(), option);

            }
            catch
            {

            }
        }

  

        private int GetMemberFromCookie()
        {
            var x = Request.Cookies["MemberId"];

            if (string.IsNullOrEmpty(x))
                return 0;


            if (int.TryParse(x, out int memberId))
                return memberId;

            return 0;
        }

        private int GetCabinFromCookie()
        {
            var x = Request.Cookies["CabinId"];

            if (string.IsNullOrEmpty(x))
                return 0;


            if (int.TryParse(x, out int cabinId))
                return cabinId;

            return 0;
        }

        private bool CheckinExists(int id)
        {
            return _context.Checkins.Any(e => e.CheckinId == id);
        }
    }
}
