using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class PutBallBackInPlay : State<GoalKeeper>
    {
        private static PutBallBackInPlay instance = new PutBallBackInPlay();
        public static PutBallBackInPlay Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
          //  throw new NotImplementedException();
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }

        public override void OnExit(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }
    }
}
