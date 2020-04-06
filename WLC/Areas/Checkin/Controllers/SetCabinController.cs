using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WLC.Areas.Checkin.Controllers
{
    public class SetCabinController : Controller
    {
        private readonly WLC.Models.WLCRacesContext _context;

      
        public SetCabinController(WLC.Models.WLCRacesContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            ViewBag.CabinList = new SelectList(_context.Cabins.ToList().OrderBy(x => x.CabinName), "CabinId", "CabinName");

            return View("Index");
        }

        public IActionResult ConfirmCabin(int CabinId )
        {
            var cabin = _context.Cabins.FirstOrDefault(x => x.CabinId == CabinId);

            if (cabin == null)
            {
                return Index();
            }

            ViewBag.CabinId = cabin.CabinId;
            ViewBag.CabinDescription = cabin.CabinName;

            return View();
        }


        public IActionResult SetCabin(int CabinId, string confirmCabin)
        {
            if (CabinId > 0 && confirmCabin == "Yes")
            {
                SetCabinCookie(CabinId);
                return LocalRedirect("~/Checkin/Index");
            }

            else
            {
                ViewBag.CabinList = new SelectList(_context.Cabins.ToList().OrderBy(x => x.CabinName), "CabinId", "CabinName");

                return View("Index");
            }
        }

        private void SetCabinCookie(int cabinId)
        {
            try
            {

                CookieOptions option = new CookieOptions();

                option.Expires = DateTime.Now.AddDays(60);

                Response.Cookies.Append("CabinId", cabinId.ToString(), option);
                Response.Cookies.Append("MemberId", "0", option);

            }
            catch (Exception ex)
            {

            }
        }
    }
}