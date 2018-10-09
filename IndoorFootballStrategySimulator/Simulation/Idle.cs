using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    class Idle:State<FieldPlayer>
    {
        private static Idle instance = new Idle();
        public static Idle Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
          
        }
        public override void Handle(FieldPlayer player)
        {
        
        }
        public override void OnExit(FieldPlayer player)
        {
        }
    }
}
