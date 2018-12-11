using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Offensive:State<Team>
    {
        private static readonly Offensive instance = new Offensive();
        static Offensive(){}
        private Offensive(){}
        public static Offensive Instance()
        {
            return instance;
        }
        public override void OnEnter(Team team)
        {
            team.TeamState = TeamState.OFFENSIVE;
            //if a player is in either the Wait or ReturnToHomeRegion states, its
            //steering target must be updated to that of its new home region to enable
            //it to move into the correct position.
            team.UpdateTargetsOfWaitingPlayers();
        }
        public override void Handle(Team team)
        {
            //if this team is no longer in control change states
            if (!team.InControl())
            {
                team.GetFSM().ChangeState(Defensive.Instance());
                return;
            }
            //calculate the best position for any supporting attacker to move to
            SupportCalculate.DetermineBestSupportingPosition();
        }
        public override void OnExit(Team team)
        {
            team.SupportingPlayer = null;
        }
    }
}
