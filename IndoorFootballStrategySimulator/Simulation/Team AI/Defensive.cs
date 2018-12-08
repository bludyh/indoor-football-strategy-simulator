using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Defensive: State<Team>
    {
        private static readonly Defensive instance = new Defensive();
        static Defensive(){}
        private Defensive(){}
        public static Defensive Instance()
        {
            return instance;
        }
        public override void OnEnter(Team team)
        {

        }
        public override void Handle(Team team)
        {

        }
        public override void OnExit(Team team)
        {
        }
    }
}
