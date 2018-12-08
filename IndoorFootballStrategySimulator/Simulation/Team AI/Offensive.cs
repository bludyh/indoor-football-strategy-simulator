using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Offensive:State<Team>
    {
        private static readonly Offensive instance = new Offensive();
        static Offensive(){}
        private Offensive(){}
        public static Offensive Instance()
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
