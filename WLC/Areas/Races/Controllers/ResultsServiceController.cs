using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WLC.Models;

namespace WLC.Areas.Races.Controllers
{
    public class ResultsServiceController : Controller
    {
        WLCRacesContext _context;

        public ResultsServiceController(WLCRacesContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("EntrantList");
        }


    }
}