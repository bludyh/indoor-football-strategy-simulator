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
            player.Steering.StartPursuit(SimulationWindow.EntityManager.Ball);
            player.Steering.Target = SimulationWindow.EntityManager.Ball.Position;
        }
        public override void Handle(FieldPlayer player)
        {
            //if the ball is within kicking range the player changes state to KickBall.
            if (player.BallWithinRange())
            {
                player.GetFSM().ChangeState(KickBall.Instance());
                return;
            }

            //if the player is the closest player to the ball then he should keep
            //chasing it
            if (player.IsClosestTeamMemberToBall())
            {
                player.Steering.StartPursuit(SimulationWindow.EntityManager.Ball);
                return;
            }
            else
            {
                player.GetFSM().ChangeState(ReturnToHomeArea.Instance());
                return;
            }
        }
        public override void OnExit(FieldPlayer player)
        {
            player.Steering.StopPursuit();
        }

        public override bool OnMessage(FieldPlayer owner, Telegram telegram)
        {
            return false;
        }
    }
}
