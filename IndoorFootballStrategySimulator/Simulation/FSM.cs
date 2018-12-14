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
        private readonly T owner;
        public State<T> CurrentState { get; private set; }
        public State<T> GlobalState { get; private set; }   
        public FSM(T owner)
        {
            this.owner = owner;
        }

        public void SetCurrentState(State<T> state)
        {
            CurrentState = state;
            CurrentState.OnEnter(owner);
        }
        public void SetGlobalState(State<T> state)
        {
            GlobalState = state;
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentState != null)
            {
                CurrentState.Handle(owner);
            }
            if (GlobalState != null)
            {
                GlobalState.Handle(owner);
            }
        }
        public void ChangeState(State<T> newState)
        {
            CurrentState.OnExit(owner);
            CurrentState = newState;
            CurrentState.OnEnter(owner);
        }
        public bool IsInState(State<T> state)
        {
            return CurrentState == state;
        }
        public bool HandleMessage(Telegram message)
        {
            if (CurrentState != null && CurrentState.OnMessage(owner, message))
            {
                return true;
            }

            if (CurrentState != null && GlobalState.OnMessage(owner, message))
            {
                return true;
            }

            return false;
        }
    }
}
