using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Idle:State<FieldPlayer>
    {
        private static readonly Idle instance = new Idle();
        static Idle(){}
        private Idle(){}
        public static Idle Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            //TODO
        }
        public override void Handle(FieldPlayer player)
        {
            //TODO
        }
        public override void OnExit(FieldPlayer player)
        {
        }
    }
}
