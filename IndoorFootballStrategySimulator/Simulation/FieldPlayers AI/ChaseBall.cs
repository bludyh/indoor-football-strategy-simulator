using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class ChaseBall:State<FieldPlayer>
    {
        private static readonly ChaseBall instance = new ChaseBall();
        static ChaseBall(){}
        private ChaseBall(){}
        public static ChaseBall Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            player.Steering.StartSeek(SimulationWindow.EntityManager.Ball.Position);
        }
        public override void Handle(FieldPlayer player)
        {
            //if the ball is within kicking range the player changes state to KickBall.
            if (player.BallWithinKickingRange())
            {
                player.GetFSM().ChangeState(KickBall.Instance());
                return;
            }

            //if the player is the closest player to the ball then he should keep
            //chasing it
            if (player.isClosestTeamMemberToBall())
            {
                player.Steering.Target = SimulationWindow.EntityManager.Ball.Position;
                return;
            }

            //if the player is not closest to the ball anymore, he should return back
            //to his home region and wait for another opportunity
            player.GetFSM().ChangeState(ReturnToHomeRegion.Instance());
        }
        public override void OnExit(FieldPlayer player)
        {
            player.Steering.StopSeek();
        }
    }
}
