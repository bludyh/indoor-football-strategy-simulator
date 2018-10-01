using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Game
{
    public class FSM<T>
    {
        private readonly T Owner;
        public State<T> CurrentState { get; private set; }
        public FSM(T owner)
        {
            Owner = owner;
        }

        public void SetCurrentState(State<T> state)
        {
            CurrentState = state;
        }
        public void Update()
        {

            if (CurrentState != null)
            {
                CurrentState.Handle(Owner);
            }
        }
    }
}
