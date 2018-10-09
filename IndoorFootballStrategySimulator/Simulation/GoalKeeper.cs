using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation
{
    class GoalKeeper : Player
    {
        //instance of the state machine class
     //   private FSM<GoalKeeper> gKeeper_stateMachine;

        private Vector2 currentPosition = new Vector2();
        public GoalKeeper(Vector2 direction,/*State<GoalKeeper> start_state,*/Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed)
        {
           // gKeeper_stateMachine = new FSM<GoalKeeper>(this);
            //gKeeper_stateMachine.SetCurrentState(start_state);
            //gKeeper_stateMachine.CurrentState.OnEnter(this);
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
            currentPosition = direction;
        }
        public override void Update(GameTime gameTime)
        {
            //run the logic for the current state
           // gKeeper_stateMachine.Update();

            //Calculate the combined force from each steering behavior
            Vector2 SteeringForce = Steering.Calculate();

            //Acceleration = Force/mass
            Vector2 acceleration = SteeringForce / Mass;

            //update velocity

            //update the position
            

            //make sure player does not exceed maximum velocity
            Velocity.Truncate(MaxSpeed);

            //update the heading if the player has a non zero velocity
            if (Velocity != null)
            {
                currentPosition = Vector2.Normalize(Velocity);

            }
            //toward to the ball
            //Check if goalkeeper has ball or not

        }
        //public FSM<GoalKeeper> GetFSM()
        //{
        //  //  return gKeeper_stateMachine;
        //}

        public Vector2 LookAt()
        {

            return currentPosition;
        }

        public void SetLookAt(Vector2 v)
        {
            currentPosition = v;
 
        }

        public Vector2 GetRearInterposeTarget() {
            
        }

    }
    
}
