using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class Offensive:State<Team>
    {
        private static Offensive instance = new Offensive();
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
