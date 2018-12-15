using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class PrepareForKickOff : State<Team>
    {
        private static readonly PrepareForKickOff instance = new PrepareForKickOff();
        static PrepareForKickOff() { }
        private PrepareForKickOff() { }
        public static PrepareForKickOff Instance(){
            return instance;
        }
        public override void Handle(Team team)
        {
            Ball ball = SimulationWindow.EntityManager.Ball;
            ball.Position = new Vector2(640f, 288f);
            ball.Trap();
            if (team.AllPlayersAtHome() && team.Opponent.AllPlayersAtHome())
            {
                team.GetFSM().ChangeState(Defensive.Instance());
            }
        }

        public override void OnEnter(Team team)
        {
            //reset Key player pointers
            team.SetControllingPlayer(null);
            team.PlayerClosestToBall = null;
            team.SupportingPlayer = null;
            team.ReceivingPlayer = null;
            team.State = TeamState.DEFENSIVE;
            team.ReturnAllPlayersToHome();
        }

        public override void OnExit(Team team)
        {
            Simulator.isGameOn = true;
        }

        public override bool OnMessage(Team owner, Telegram telegram)
        {
            return false;
        }
    }
}
