using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class ReturnHome : State<GoalKeeper>
    {
        private static ReturnHome instance = new ReturnHome();
        public static ReturnHome Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
            //  owner.Steering.Target = owner.HomeRegion.Center;

            //if close enough to home or the opponents get control
            //over the ball and change state to tend goal
            if (!owner.Team.InControl() /*&& owner.InHomeRegion*/) {
                FSM<GoalKeeper> newState = new FSM<GoalKeeper>(owner);
                newState.ChangeState(TendGoal.Instance());
            }
        }

        public override void OnEnter(GoalKeeper owner)
        {
            owner.Steering.StartArrival(owner.Team.Goal.Center);
        }

        public override void OnExit(GoalKeeper owner)
        {
            owner.Steering.StopArrival();
        }
    }
}
