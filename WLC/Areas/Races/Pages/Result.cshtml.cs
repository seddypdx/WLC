using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using WLC.Models;

namespace WLC.Areas.Races.Pages
{
    public class ResultModel : PageModel
    {

        private WLCRacesContext _context;
        public SelectList RaceList;
        public IQueryable<Racers> AvailableRacers;
        public IQueryable<Results> Results;
  

        [BindProperty]
        public int ActiveRaceId { get; set; } 

        public WLC.Models.Races ActiveRace;

        public ResultModel(WLCRacesContext context)
        {
            _context = context;
            RaceList = new SelectList(_context.Races.Where(x => x.IsBoating == true).ToList().OrderBy(x => x.SortOrder), "RaceId", "RaceName");
 
            ActiveRaceId = 200;
        }

        public void OnGet(int raceId, int racerId)
        {
            if (raceId > 0)
                ActiveRaceId = raceId;

            if (ActiveRaceId == 0)
                ActiveRaceId = 200;

            if (racerId > 0)
            {
                var result = new Results()
                {
                    Place = 1,
                    TeamId =1,
                    RaceId = ActiveRaceId,
                    RacerId = racerId,
                    Year = 2019
                    
                };
                _context.Results.Add(result);
                _context.SaveChanges();
            }
            ViewData["RaceId"] = ActiveRaceId;

            ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            SetupDetails();
        }

        public void OnPost()
        {
            ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            ViewData["RaceId"] = ActiveRaceId;

            SetupDetails();

        }

        private void SetupDetails()
        {
            AvailableRacers = _context.Racers
                                       .Where(x => x.Age >= ActiveRace.MinimumAge
                                            && x.Age <= ActiveRace.MaximumAge
                                            && (x.BoyOrGirl == ActiveRace.RaceBoyOrGirl || ActiveRace.RaceBoyOrGirl == "b/g") )
                                        .Include(x => x.Cabin)

                                            .OrderBy(x => x.LastName);

            Results = _context.Results.Where(x => x.Year == 2019 && x.RaceId == ActiveRaceId)
                                      .Include(x => x.Racer).ThenInclude(x => x.Cabin)
                                      .OrderBy(x => x.Place);


        }

        public PartialViewResult OnGetEntrantsPartial(int raceId)
        {
             if (raceId > 0)
            {
                ActiveRaceId = raceId;
                ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            }
            ViewData["RaceId"] = raceId;


            SetupDetails();

            return new PartialViewResult
            {
                ViewName = "_ResultEntrants",
                ViewData = new ViewDataDictionary<IQueryable<Results>>(ViewData, Results)
            };
        }

        public PartialViewResult OnGetQualifiedPartial(int raceId)
        {
            ViewData["RaceId"] = raceId;

            if (raceId > 0)
            {
                ActiveRaceId = raceId;
                ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            }
   

            SetupDetails();

              return new PartialViewResult
            {
                ViewName = "_ResultQualified",
                ViewData = new ViewDataDictionary<IQueryable<Racers>>(ViewData, AvailableRacers)
            };
        }

        public PartialViewResult OnGetEnterRacer(int raceId, int racerId)
        {

            if (racerId > 0)
            {
                var result = new Results()
                {
                    Place = 1,
                    TeamId = 1,
                    RaceId = raceId,
                    RacerId = racerId,
                    Year = 2019

                };
                _context.Results.Add(result);
                _context.SaveChanges();
            }

            ViewData["RaceId"] = raceId;

            SetupDetails();

            return new PartialViewResult
            {
                ViewName = "_ResultEntrants",
                ViewData = new ViewDataDictionary<IQueryable<Results>>(ViewData, Results)
            };
        }
    }
}