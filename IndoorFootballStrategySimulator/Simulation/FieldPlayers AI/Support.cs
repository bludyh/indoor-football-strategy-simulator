using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class Support: State<FieldPlayer>
    {
        private static Support instance = new Support();
        public static Support Instance()
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
