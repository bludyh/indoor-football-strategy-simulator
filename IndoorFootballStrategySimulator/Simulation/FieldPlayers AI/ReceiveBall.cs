using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class ReceiveBall : State<FieldPlayer>
    {
        private static readonly ReceiveBall instance = new ReceiveBall();
        static ReceiveBall() { }
        private ReceiveBall() { }
        public static ReceiveBall Instance()
        {
            return instance;
        }
        public override void Handle(FieldPlayer owner)
        {
            var ball = SimulationWindow.EntityManager.Ball;
            //if the ball comes close enough to the player or if his team lose control
            //he should change state to chase the ball
            if (owner.BallWithinReceivingRange() || !owner.Team.InControl())
            {
                owner.GetFSM().ChangeState(ChaseBall.Instance());
                return;
            }
            if (owner.Steering.PursuitIsOn())
            {
                owner.Steering.Target = ball.Position;
            }

            //if the player has 'arrived' at the steering target he should wait and
            //turn to face the ball
            if (owner.AtTarget())
            {
                owner.Steering.StopArrival();
                owner.Steering.StopPursuit();
                owner.TrackBall();
                owner.Velocity = new Vector2(0, 0);
            }
        }

        public override void OnEnter(FieldPlayer owner)
        {
            // let the team know this player is receiving the ball
            owner.Team.ReceivingPlayer = owner;
            //this player is also now the controlling player
            owner.Team.SetControllingPlayer(owner);
            //there are two types of receive behavior. One uses arrive to direct
            //the receiver to the position sent by the passer in its telegram. The
            //other uses the pursuit behavior to pursue the ball. 
            //This statement selects between them dependent on the probability
            //ChanceOfUsingArriveTypeReceiveBehavior, whether or not an opposing
            //player is close to the receiving player, and whether or not the receiving
            //player is in the opponents 'hot region' (the third of the pitch closest
            //to the opponent's goal
            float PassThreatRadius = 70f;
            if ((owner.InHotRegion() || SupportCalculate.RandFloat() < 0.5f)
                    && !owner.Team.IsOpponentWithinRadius(owner.Position, PassThreatRadius))
            {
                //owner.Steering.StartArrival();
            }
            else
            {
                owner.Steering.StartPursuit(SimulationWindow.EntityManager.Ball);
            }

        }

        public override void OnExit(FieldPlayer owner)
        {
            owner.Steering.StopArrival();
            owner.Steering.StopPursuit();
            owner.Team.ReceivingPlayer = null;
        }
    }
}
