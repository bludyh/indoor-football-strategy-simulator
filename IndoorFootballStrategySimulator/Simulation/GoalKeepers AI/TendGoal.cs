using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class TendGoal : State<GoalKeeper>
    {
        private static readonly TendGoal instance = new TendGoal();
        // Explicit static constructor to tell C# compiler
        static TendGoal(){}
        // 
        private TendGoal(){}
        public static TendGoal Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
            var field = SimulationWindow.EntityManager.Field;
            var ball = SimulationWindow.EntityManager.Ball;

            //The rear interpose target will change as the ball position changes
            //owner.Steering.Target = owner.GetRearInterposeTarget();

            //if the ball comes in range the keeper traps t and then changes state to put
            //ball back in play
            if (owner.BallWithinKeeperRange()) {
                ball.Trap();
                field.GoalKeeperHasBall = true;
                owner.GetFSM().ChangeState(PutBallBackInPlay.Instance());
                return;
            }

            //if ball is within a predefined distance, keeper moves out from 
            //position to try and intercept it
            if (owner.BallWithinRangeForIntercept() && !owner.Team.InControl()) {
                owner.GetFSM().ChangeState(InterceptBall.Instance());
            }

            //if keeper is too far away from goal, he should go back to goal Area
            if (owner.TooFarFromGoalMouth() && owner.Team.InControl()) {
                owner.GetFSM().ChangeState(ReturnHome.Instance());
                return;
            }

        }
             
        public override void OnEnter(GoalKeeper owner)
        {
            //interpose will position the agent between the ball position and a target
            //position situated along the goal mouth.
            owner.Steering.Target = owner.GetRearInterposeTarget();
            //turn on interpose
            owner.Steering.StartInterpose(SimulationWindow.EntityManager.Ball, owner.Steering.Target);
        }

        public override void OnExit(GoalKeeper owner)
        {
           owner.Steering.StopInterpose();
        }

        public override bool OnMessage(GoalKeeper owner, Telegram telegram)
        {
            return false;
        }
    }
}
