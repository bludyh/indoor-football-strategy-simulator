using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class InterceptBall : State<GoalKeeper>
    {
        private static readonly InterceptBall instance = new InterceptBall();
        static InterceptBall(){}
        private InterceptBall(){}
        public static InterceptBall Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
           // throw new NotImplementedException();
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
