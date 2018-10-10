using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class ChaseBall:State<FieldPlayer>
    {
        private static ChaseBall instance = new ChaseBall();
        public static ChaseBall Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            //player.Steering.StartSeek();
        }
        public override void Handle(FieldPlayer player)
        {
            throw new NotImplementedException();
        }
        public override void OnExit(FieldPlayer player)
        {
            player.Steering.StopSeek();
        }
    }
}
