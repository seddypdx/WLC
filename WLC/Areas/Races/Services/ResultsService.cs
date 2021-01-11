using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WLC.Models;

namespace WLC.Areas.Races.Services
{
    public class TeamGroup
    {
        public IList<Results> TeamMembers { get; set; }
        public int Place { get; set; }
        public int Members { get; set; }
        public int DisqualifiedCount { get; set; }
        public int PointsPlace { get; set; }
        public string Ribbon { get; set; }
    };


    public static class ResultsService
    {
        public static void AssignPoints(WLCRacesContext context, int year, WLC.Models.Races race)
        {
  
            SetQualifiedStatus(context, year, race);

 
            var teams_ = from t in context.Results
                        where t.Year == year && t.RaceId == race.RaceId
                        group t by t.TeamId into TeamGroup
                        select new TeamGroup()
                        {
                            TeamMembers = TeamGroup.ToList(),
                            Place = (int) TeamGroup.Max(x => x.Place),
                            Members = TeamGroup.Count(x => x.Racer.MemberStatusId == (int)Models.MemberStatusEnum.Member),
                            DisqualifiedCount = TeamGroup.Count(x => x.Comments == "Not Qualified")
                        };


            var teams = teams_.ToList();

            // cyclte through the teamsand assign a place depending on member status
            int order = 1;
            int guestAdder = 0;
            int tieCount = 0;
            int previousPlace = 0;
            
            foreach(var team in teams.OrderBy(x => x.Place))
            {
                if (previousPlace == team.Place) // Tie
                    tieCount--;
 
                if (team.Members == 0) // no memebers
                        team.PointsPlace = (int)(order + tieCount + guestAdder);
                else  
                        team.PointsPlace = (int)(order + tieCount);



                if (team.Members == 0)
                    guestAdder++;
                else
                {
                    order++;
                    previousPlace = (int)team.Place; // only worry about ties with members

                }

            }


            // set the ribons and assign points and other values down to the individual
            foreach (var team in teams.OrderBy(x => x.Place))
            {
                if (team.DisqualifiedCount > 0)
                    team.Ribbon = "Purple"; // This used to say "No ribon"  but decided to give ribon anyway
                else
                    team.Ribbon = race.GetColorForPlace(team.PointsPlace);


                foreach (var teamMember in team.TeamMembers)
                {
                    teamMember.Ribbon = team.Ribbon;
                    teamMember.PointsPlace = team.PointsPlace;

                    if (team.DisqualifiedCount > 0 ||  teamMember.Racer.MemberStatusId != (int)Models.MemberStatusEnum.Member || teamMember.Racer.GetAge(DateTime.Now) > 18)
                        teamMember.Points = 0;
                    else
                        teamMember.Points = race.GetPointsForRibon(teamMember.Ribbon);

                    context.Results.Attach(teamMember);
                    context.Entry(teamMember).State = EntityState.Modified;
                    context.SaveChanges();

                }
            }


       }


        private static void SetQualifiedStatus(WLCRacesContext context, int year, WLC.Models.Races race)
        {

            var entries = context.Results
                        .Where(x => x.Year == year && x.RaceId == race.RaceId)
                        .Include(x => x.Race)
                        .Include(x => x.Racer).ThenInclude(x => x.MemberStatus)
                        .OrderBy(x => x.Place).ThenBy(z => z.TeamId).ThenBy(y => y.Racer.MemberStatus.MemberStatus);

            foreach (var entry in entries)
            {
                if (entry.Race.RacePoints == "Participation Ribbon Only" || entry.Race.RacePoints == "Just for Fun: 0 Points")
                    entry.Comments = entry.Race.RacePoints;
                else
                {
                    entry.Comments = "! Member";
                    if (entry.Racer.GetAge(DateTime.Now) > entry.Race.MaximumAge
                        || (entry.Racer.GetAge(DateTime.Now) < entry.Race.MinimumAge && entry.Racer.MemberStatus.MemberStatus != "* Retiree")
                        || ((entry.Race.RaceBoyOrGirl != entry.Racer.BoyOrGirl) && entry.Race.RaceBoyOrGirl != "b/g"))
                        entry.Comments = "Not Qualified";
                    else
                        if (entry.Racer.MemberStatus.MemberStatusId != (int)Models.MemberStatusEnum.Member)
                           entry.Comments = entry.Racer.MemberStatus.MemberStatus;


                }
                context.Update(entry);
            }

            context.SaveChanges();
        }
    }
}
