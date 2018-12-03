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
           // throw new NotImplementedException();
           
        }

        public override void OnEnter(GoalKeeper owner)
        {
            
            //owner.Steering.StartPursuit(MovingEntity ball){

            //}
            //throw new NotImplementedException();
        }

        public override void OnExit(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }
    }
}
