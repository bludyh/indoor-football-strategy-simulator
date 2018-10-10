using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class Dribble: State<FieldPlayer>
    {
        private static Dribble instance = new Dribble();
        public static Dribble Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
           
        }
        public override void Handle(FieldPlayer player)
        {
            throw new NotImplementedException();
        }
        public override void OnExit(FieldPlayer player)
        {
            
        }
    }
}
