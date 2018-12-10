using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation
{
    [DataContract]
    public class FieldPlayer : Player
    {

        private FSM<FieldPlayer> fpStateMachine;

        [DataMember]
        public int OffensiveHomeArea { get; set; }

        [DataMember]
        public List<int> OffensiveAreas { get; set; }

        [DataMember]
        public int DefensiveHomeArea { get; set; }

        [DataMember]
        public List<int> DefensiveAreas { get; set; }
        public static PlayerRole Role { get; private set; }

        public FieldPlayer(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed,
            Team team= null, int offHomeArea = -1, List<int> offAreas = null, int defHomeArea = -1, List<int> defAreas = null, State<FieldPlayer> startState = null) 
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed, team, Role)
        {
            fpStateMachine = new FSM<FieldPlayer>(this);
            OffensiveHomeArea = offHomeArea;
            OffensiveAreas = offAreas;
            DefensiveHomeArea = defHomeArea;
            DefensiveAreas = defAreas;
            if (startState != null)
            {
                fpStateMachine.SetCurrentState(startState);
                fpStateMachine.CurrentState.OnEnter(this);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            fpStateMachine.Update(gameTime);
        }
        public FSM<FieldPlayer> GetFSM()
        {
            return fpStateMachine;
        }

    }
}
