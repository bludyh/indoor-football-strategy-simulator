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
            //The rear interpose target will change as the ball position changes
            owner.Steering.Target = owner.GetRearInterposeTarget();

            //if the ball comes in range the keeper traps t and then changes state to put
            //ball back in play
            if (owner.BallWithinKeeperRange()) {
                owner.Field.GoalKeeperHasBall = true;
                owner.GetFSM().ChangeState(PutBallBackInPlay.Instance());
            }

            //if ball is within a predefined distance, keeper moves out from 
            //position to try and intercept it
            if (owner.BallWithinRangeForIntercept() && !owner.Team.InControl()) {
                owner.GetFSM().ChangeState(InterceptBall.Instance());
            }

            //if keeper is too far away from goal, he should go back to goal region
            if (owner.TooFarFromGoalMouth() && owner.Team.InControl()) {
                owner.GetFSM().ChangeState(ReturnHome.Instance());
            }

        }
             
        public override void OnEnter(GoalKeeper owner)
        {
            //turn on interpose
            // TODO
            owner.Steering.Target = owner.GetRearInterposeTarget();
        }

        public override void OnExit(GoalKeeper owner)
        {
            owner.Steering.StopInterpose();
        }
    }
}
