using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Game
{
    class GlobalKeeperState : State<GoalKeeper>
    {
        private GlobalKeeperState globalState = new GlobalKeeperState();

        private GlobalKeeperState()
        {
        }

        public GlobalKeeperState State() {
            return globalState;
        }
        
            
       
        public override void Handle(GoalKeeper owner)
        {
            //throw new NotImplementedException();
           
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
