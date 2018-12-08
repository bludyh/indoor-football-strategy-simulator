using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class GoalKeeper : Player
    {

        private FSM<GoalKeeper> gkStateMachine;

        public Area HomeArea { get; set; }
        public List<Area> Areas { get; set; }

        public GoalKeeper(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed,
            Team team = null, Area homeArea = null, List<Area> areas = null, State<GoalKeeper> startState = null)
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed, team)
        {
            gkStateMachine = new FSM<GoalKeeper>(this);
            HomeArea = homeArea;
            Areas = areas;
            if (startState != null)
            {
                gkStateMachine.SetCurrentState(startState);
                gkStateMachine.CurrentState.OnEnter(this);
            }
        }
        public override void Update(GameTime gameTime)
        {
            BounceBall();
        }
        public FSM<GoalKeeper> GetFSM()
        {
            return gkStateMachine;
        }
        private void BounceBall()
        {
            if (SimulationWindow.EntityManager.Ball.Position.Y > 176 && SimulationWindow.EntityManager.Ball.Position.Y <400)
            {
            this.Position = new Vector2(this.Position.X, SimulationWindow.EntityManager.Ball.Position.Y);
            }
            foreach (var entity in SimulationWindow.EntityManager.Entities)
            {
                if (entity is Ball ball)
                {
                    Vector2 offset = ball.Position - Position;
                    float overlap = Radius + ball.Radius - offset.Length();
                    if (overlap >= 0)
                        ball.Velocity += Vector2.Normalize(offset) * overlap;
                }
            }
        }
    }
}

