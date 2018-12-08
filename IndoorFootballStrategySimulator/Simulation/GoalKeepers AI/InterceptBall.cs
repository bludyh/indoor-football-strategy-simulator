using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class InterceptBall : State<GoalKeeper>
    {
        private static InterceptBall instance = new InterceptBall();
        public static InterceptBall Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
            FSM<GoalKeeper> newState = new FSM<GoalKeeper>(owner);
            // if the goalkeeper moves too far away from the goal, he should return to his
            //home region and he is not the person who is the closest to the ball
            //Then he should keep trying to intercep it.
            if (owner.TooFarFromGoalMouth() && !owner.isClosetPlayerOnPitchToBall()) {
               
                newState.ChangeState(ReturnHome.Instance());
                return;
            }
            //if the ball becomes in range of the goalkeeper's hands, he puts the ball bak in play
            if (owner.BallWithinKeeperRange()) {
                owner.Field.GoalKeeperHasBall = true;
                newState.ChangeState(PutBallBackInPlay.Instance());
                return;
            }

            
        }

        public override void OnEnter(GoalKeeper owner)
        {
            owner.Steering.StartPursuit(owner);

        }

        public override void OnExit(GoalKeeper owner)
        {
            owner.Steering.StopPursuit();
        }
    }
}
