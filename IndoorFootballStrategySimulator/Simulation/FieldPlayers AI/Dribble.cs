using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Dribble: State<FieldPlayer>
    {
        private static readonly Dribble instance = new Dribble();
        static Dribble(){}
        private Dribble(){}
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
