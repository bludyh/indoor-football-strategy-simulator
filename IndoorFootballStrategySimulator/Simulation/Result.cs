using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class Result
    {
        // Properties
        public Strategy HomeTeamStrategy { get; set; }
        public Strategy AwayTeamStrategy { get; set; }

        public int ScoreHomeTeam { get; set; }
        public int ScoreAwayTeam { get; set; }

        // Constructor
        public Result(Strategy home, Strategy away, int scoreHome, int scoreAway)
        {
            HomeTeamStrategy = home;
            AwayTeamStrategy = away;
            ScoreHomeTeam = scoreHome;
            ScoreAwayTeam = scoreAway;
        }

        public override string ToString()
        {
            string returnString = String.Format("{0} {1} - {2} {3}", HomeTeamStrategy.Name, ScoreHomeTeam, ScoreAwayTeam, AwayTeamStrategy.Name);
            return returnString;
        }

        public List<string> ToListViewRow()
        {
            List<string> row = new List<string>();
            row.Add(HomeTeamStrategy.Name);
            row.Add(ScoreHomeTeam.ToString());
            row.Add(ScoreAwayTeam.ToString());
            row.Add(AwayTeamStrategy.Name);
            return row;
        }
    }
}
