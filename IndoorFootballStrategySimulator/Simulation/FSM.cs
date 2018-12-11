using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
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
        public void Update(GameTime gameTime)
        {
            if (CurrentState != null)
            {
                CurrentState.Handle(Owner);
            }
        }
        public void ChangeState(State<T> newState)
        {
            CurrentState.OnExit(Owner);
            CurrentState = newState;
            CurrentState.OnEnter(Owner);
        }
        public bool IsInState(State<T> state)
        {
            return CurrentState == state;
        }
    }
}
