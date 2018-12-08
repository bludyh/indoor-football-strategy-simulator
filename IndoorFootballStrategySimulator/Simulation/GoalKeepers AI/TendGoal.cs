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
            //throw new NotImplementedException();
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //owner.Steering.StartInterpose(owner, SimulationWindow.EntityManager.Ball);
        }

        public override void OnExit(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }
    }
}
