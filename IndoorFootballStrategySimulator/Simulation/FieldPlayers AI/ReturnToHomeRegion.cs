using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class ReturnToHomeRegion : State<FieldPlayer>
    {
        private static readonly ReturnToHomeRegion instance = new ReturnToHomeRegion();
        static ReturnToHomeRegion() { }
        private ReturnToHomeRegion() { }
        public static ReturnToHomeRegion Instance()
        {
            return instance;
        }
        public override void Handle(FieldPlayer owner)
        {
            throw new NotImplementedException();
        }

        public override void OnEnter(FieldPlayer owner)
        {
            throw new NotImplementedException();
        }

        public override void OnExit(FieldPlayer owner)
        {
            throw new NotImplementedException();
        }
    }
}
