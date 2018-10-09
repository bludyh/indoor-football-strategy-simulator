using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Game.GoalKeeper.GoalKeeperState
{
    class TendGoal : State<GoalKeeper>
    {
        public override void Handle(GoalKeeper owner)
        {
           // throw new NotImplementedException();
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //this is the distance the keeper puts between the back of the net 
            //and the ball when using the interpose steering behavior
            owner.Steering.InterposeOn(20.0);
        }

        public override void OnExit(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }
    }
}
