using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Game
{
    abstract class State
    {
        public abstract void OnEnter<T>(T owner);
        public abstract void Handle<T>(T owner);
        public abstract void OnExit<T>(T owner);
    }
}
