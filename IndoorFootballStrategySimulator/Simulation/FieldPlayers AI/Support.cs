using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Support: State<FieldPlayer>
    {
        private static readonly Support instance = new Support();
        static Support(){}
        private Support(){}
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
