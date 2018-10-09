using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public abstract class State<T>
    {
        public abstract void OnEnter(T owner);
        public abstract void Handle(T owner);
        public abstract void OnExit(T owner);
    }
}
