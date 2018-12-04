using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class FieldPlayer : Player
    {
        private readonly FSM<FieldPlayer> fpStateMachine;
        public FieldPlayer(Team team, State<FieldPlayer> startState,Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed) 
            : base(team,texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed)
        {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
            fpStateMachine = new FSM<FieldPlayer>(this);
            if (startState != null)
            {
                fpStateMachine.SetCurrentState(startState);
                fpStateMachine.CurrentState.OnEnter(this);
            }
        }
        public override void Update(GameTime gameTime)
        {
        }
        public FSM<FieldPlayer> GetFSM()
        {
            return fpStateMachine;
        }

    }
}
