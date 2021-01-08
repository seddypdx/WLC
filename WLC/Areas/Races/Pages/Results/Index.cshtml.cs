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
using WLC.Services;

namespace WLC.Areas.Races.Pages.Results
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ResultsModel : PageModel
    {

        private readonly WLCRacesContext _context;
        public SelectList RaceList;
        public SelectList CabinList;
        public SelectList MemberStatusList;

        public IQueryable<WLC.Models.Racers> AvailableRacers;
        public IQueryable<WLC.Models.Results> Results;

        [BindProperty]
        public bool IsBooting { get; set; }


        [BindProperty(SupportsGet =true)]
        public int ActiveRaceId { get; set; }

        [BindProperty]
        public WLC.Models.Racers NewRacer { get; set; }

        public WLC.Models.Races ActiveRace;

        public ResultsModel(WLCRacesContext context)
        {
            _context = context;
 
            ActiveRaceId = 0;
        }

        public void OnGet()
        {
            SetupRaceList();

            if (ActiveRaceId == 0)
                ActiveRaceId = System.Convert.ToInt32(RaceList.FirstOrDefault()?.Value);

                ViewData["RaceId"] = ActiveRaceId;

            ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);
            ViewData["TeamRace"] = ActiveRace.Participants > 1;

            SetupDetails();
        }

        public void OnPost()
        {
            SetupRaceList();

            if (ActiveRaceId == 0)
                ActiveRaceId = System.Convert.ToInt32(RaceList.FirstOrDefault()?.Value);

            ActiveRace = _context.Races.FirstOrDefault(x => x.RaceId == ActiveRaceId);

            ViewData["RaceId"] = ActiveRaceId;
            ViewData["TeamRace"] = ActiveRace?.Participants > 1;


            SetupDetails();

        }

        private void SetupRaceList()
        {

            RaceList = new SelectList(_context.Races.Where(x => x.IsBoating == IsBooting).ToList().OrderBy(x => x.RaceOrder), "RaceId", "RaceName");

        }

        private void SetupDetails()
        {
            CabinList = new SelectList(_context.Cabins.ToList().OrderBy(x => x.CabinName), "CabinId", "CabinName");
            MemberStatusList = new SelectList(_context.MemberStatuses.ToList().OrderBy(x => x.MemberStatus), "MemberStatusId", "MemberStatus");


            Results = _context.Results.Where(x => x.Year == Globals.GetActiveYear(HttpContext) && x.RaceId == ActiveRaceId)
                                      .Include(x => x.Racer).ThenInclude(x => x.Cabin)
                                      .OrderBy(x => x.Place).ThenBy(x => x.TeamId);


            AvailableRacers = _context.Racers
                           .Where(x => x.GetAge(DateTime.Now) >= ActiveRace.MinimumAge
                                && x.GetAge(DateTime.Now) <= ActiveRace.MaximumAge
                                && (x.BoyOrGirl == ActiveRace.RaceBoyOrGirl || ActiveRace.RaceBoyOrGirl == "b/g")
                                && !_context.Results.Any(p => p.RaceId==ActiveRace.RaceId && p.RacerId == x.RacerId && p.Year ==2019)
                                && x.MemberStatusId != (int) MemberStatusEnum.Inactive
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

        public IActionResult OnGetRacer(int racerId)
        {
            try
            {
                var racer = _context.Racers.FirstOrDefault(x => x.RacerId == racerId);

                return new JsonResult(new { error = false, Racer = racer });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = true, message = "Getting Racer Failed: " + ex.Message });

            }

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


        public PartialViewResult OnGetQualifiedAll(int raceId)
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

            AvailableRacers = _context.Racers
                       .Where(x =>  x.MemberStatusId != (int)MemberStatusEnum.Inactive
                            )
                        .Include(x => x.Cabin)
                        .OrderBy(x => x.LastName);


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
                    Year = Globals.GetActiveYear(HttpContext)

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
                if (addTeamRequest.RaceId == 0)
                    throw new Exception("Invalid Race");

                var teamId = GetNextTeamId(addTeamRequest.RaceId);

                foreach (int racerId in addTeamRequest.RacerIds)
                {
                    var result = new WLC.Models.Results()
                    {
                        Place = 4,
                        TeamId = teamId,
                        RaceId = addTeamRequest.RaceId,
                        RacerId =racerId,
                        Year = Globals.GetActiveYear(HttpContext)

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
            var lastResult = _context.Results.Where(x => x.Year == Globals.GetActiveYear(HttpContext)
                                                        && x.RaceId == raceId)
                                         .OrderByDescending(x => x.TeamId).FirstOrDefault();

            if (lastResult != null)
               return (int)lastResult.TeamId + 1;

            return 1;
        }
        public class AddTeamRequest
        {
            public int RaceId { get; set; }
            public int[] RacerIds { get; set; }
        }

        public class SaveRacerRequest
        {
            public int RacerId { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string BoyOrGirl { get; set; }
            public DateTime Birthdate{ get; set; }
            public int Age { get; set; }
            public int MemberStatusId { get; set; }
            public int CabinId { get; set; }

        }

        public IActionResult OnGetSetRacerPosition([FromQuery] int teamId, int raceId, int place)
        {

            try
            {

                var results = _context.Results.Where(x => x.Year == Globals.GetActiveYear(HttpContext) && x.TeamId == teamId && x.RaceId == raceId);
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
     
                var results = _context.Results.Where(x => x.Year== Globals.GetActiveYear(HttpContext) && x.TeamId == teamId && x.RaceId==raceId);
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

          public IActionResult OnPostSaveRacer([FromBody] [Bind("RacerId, FirstName, LastName, BoyOrGirl ,Birthdate,Age, MemberStatusId, CabinId")] Models.Racers saveRacer)
        {

            try
            {
               // if (saveRacer.Birthdate.HasValue)
               //     saveRacer.SetAge();
                if (saveRacer.RacerId == 0)
                    _context.Racers.Add(saveRacer);
                else
                {
                    var updateRacer = _context.Racers.FirstOrDefault(x => x.RacerId == saveRacer.RacerId);
                    updateRacer.FirstName = saveRacer.FirstName;
                    updateRacer.LastName = saveRacer.LastName;
                    updateRacer.MemberStatusId = saveRacer.MemberStatusId;
                    updateRacer.Birthdate = saveRacer.Birthdate;
                    updateRacer.CabinId = saveRacer.CabinId;
                    updateRacer.BoyOrGirl = saveRacer.BoyOrGirl;
                   // if (updateRacer.Birthdate == null)
                  //      updateRacer.Age = saveRacer.Age;
                  //  else
                  //     updateRacer.SetAge();

                    _context.Racers.Update(updateRacer);
                }

                _context.SaveChanges();
                return new JsonResult(new { error = false, message = "RacerSaved" });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = true, message = "Adding Racer Failed: " + ex.Message });

            }
        }


        #endregion

    }
}