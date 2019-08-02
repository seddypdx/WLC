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

namespace WLC.Areas.Races.Pages.Results
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class PointsModel : PageModel
    {

        private WLCRacesContext _context;
        public SelectList RaceList;
        public IQueryable<WLC.Models.Racers> AvailableRacers;
        public IQueryable<WLC.Models.Results> Results;

        [BindProperty]
        public bool IsBooting { get; set; }


        [BindProperty(SupportsGet=true)]
        public int ActiveRaceId { get; set; }

        [BindProperty]
        public WLC.Models.Racers NewRacer { get; set; }

        public WLC.Models.Races ActiveRace;

        public PointsModel(WLCRacesContext context)
        {
            _context = context;
 
            ActiveRaceId = 200;
        }

        public void OnGet()
        {
            RaceList = new SelectList(_context.Races.Where(x => x.IsBoating == IsBooting).ToList().OrderBy(x => x.SortOrder), "RaceId", "RaceName");

            if (ActiveRaceId== 0)
                ActiveRaceId = System.Convert.ToInt32(RaceList.FirstOrDefault()?.Value);

                 ViewData["RaceId"] = ActiveRaceId;

            ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            ViewData["TeamRace"] = ActiveRace.Participants > 1;

            SetupDetails();
        }

        public void OnPost()
        {
            ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            ViewData["RaceId"] = ActiveRaceId;
            ViewData["TeamRace"] = ActiveRace.Participants > 1;

            RaceList = new SelectList(_context.Races.Where(x => x.IsBoating == IsBooting).ToList().OrderBy(x => x.SortOrder), "RaceId", "RaceName");

            SetupDetails();

        }

        private void SetupDetails()
        {
            Services.ResultsService.AssignPoints(_context, 2019, ActiveRace);

            Results = _context.Results.Where(x => x.Year == 2019 && x.RaceId == ActiveRaceId)
                                      .Include(x => x.Racer).ThenInclude(x => x.Cabin)
                                      .OrderBy(x => x.Place).ThenBy(x => x.TeamId);


            AvailableRacers = _context.Racers
                           .Where(x => x.Age >= ActiveRace.MinimumAge
                                && x.Age <= ActiveRace.MaximumAge
                                && (x.BoyOrGirl == ActiveRace.RaceBoyOrGirl || ActiveRace.RaceBoyOrGirl == "b/g")
                                && !_context.Results.Any(p => p.RaceId==ActiveRace.RaceId && p.RacerId == x.RacerId)
                                )
                            .Include(x => x.Cabin)
                            .OrderBy(x => x.LastName);
        }

        public PartialViewResult OnGetEntrantsPartial(int raceId)
        {
            ViewData["TeamRace"] = false;
             if (raceId > 0)
            {
                ActiveRaceId = raceId;
                ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
                ViewData["TeamRace"] = ActiveRace.Participants > 1;
            }
            ViewData["RaceId"] = raceId;


            SetupDetails();

            return new PartialViewResult
            {
                ViewName = "_ResultEntrants",
                ViewData = new ViewDataDictionary<IQueryable<WLC.Models.Results>>(ViewData, Results)
            };
        }

        public PartialViewResult OnGetQualifiedPartial(int raceId)
        {
            ViewData["RaceId"] = raceId;
            ViewData["TeamRace"] = false;

            if (raceId > 0)
            {
                ActiveRaceId = raceId;
                ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
                ViewData["TeamRace"] = ActiveRace.Participants > 1;
            }


            SetupDetails();

              return new PartialViewResult
            {
                ViewName = "_ResultQualified",
                ViewData = new ViewDataDictionary<IQueryable<WLC.Models.Racers>>(ViewData, AvailableRacers)
            };
        }

        #region ajaxhandlers

        public IActionResult OnGetEnterRacer([FromQuery] int racerId, int raceId)
        {

            try
            {
                if (racerId == 0)
                    throw new Exception("Invalid Race");

                var result = new WLC.Models.Results()
                {
                    Place = 4,
                    TeamId = GetNextTeamId(raceId),
                    RaceId = raceId,
                    RacerId = racerId,
                    Year = 2019

                };
                _context.Results.Add(result);
                _context.SaveChanges();
                return new JsonResult(new { error = false, message = "RacerAdded" });

            }
            catch(Exception ex)
            {
                return new JsonResult(new { error = true, message = "Adding Racer Failed: " + ex.Message });

            }
        }

        public IActionResult OnPostEnterTeam([FromBody] AddTeamRequest addTeamRequest)
        {

            try
            {
                if (addTeamRequest.raceId == 0)
                    throw new Exception("Invalid Race");

                var teamId = GetNextTeamId(addTeamRequest.raceId);

                foreach (int racerId in addTeamRequest.racerIds)
                {
                    var result = new WLC.Models.Results()
                    {
                        Place = 4,
                        TeamId = teamId,
                        RaceId = addTeamRequest.raceId,
                        RacerId =racerId,
                        Year = 2019

                    };
                    _context.Results.Add(result);

                }
                _context.SaveChanges();
                return new JsonResult(new { error = false, message = "RacerAdded" });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = true, message = "Adding Team Failed: " + ex.Message });

            }
        }

        private int GetNextTeamId(int raceId)
        {
            // get the highest team Id for this race
            var lastResult = _context.Results.Where(x => x.Year == 2019
                                                        && x.RaceId == raceId)
                                         .OrderByDescending(x => x.TeamId).FirstOrDefault();

            if (lastResult != null)
               return (int)lastResult.TeamId + 1;

            return 1;
        }
        public class AddTeamRequest
        {
            public int raceId { get; set; }
            public int[] racerIds { get; set; }
        }

        public IActionResult OnGetSetRacerPosition([FromQuery] int teamId, int raceId, int place)
        {

            try
            {

                var results = _context.Results.Where(x => x.Year == 2019 && x.TeamId == teamId && x.RaceId == raceId);
                foreach (var result in results)
                {
                    result.Place = place;
                    _context.Update(result);
                }
                _context.SaveChanges();
                return new JsonResult(new { error = false, message = "Racer updated" });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = true, message = "Setting position failed: " + ex.Message });

            }



        }

        public IActionResult OnGetRemoveRacer([FromQuery] int teamId, int raceId)
        {

            try
            {
     
                var results = _context.Results.Where(x => x.Year==2019 && x.TeamId == teamId && x.RaceId==raceId);
                foreach(var result in results)
                     _context.Remove(result);

                _context.SaveChanges();
                return new JsonResult(new { error = false, message = "Racer Removed" });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = true, message = "Removing Racer Failed: " + ex.Message });

            }



        }

        #endregion

    }
}