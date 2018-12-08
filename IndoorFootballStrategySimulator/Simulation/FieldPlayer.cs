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

        private FSM<FieldPlayer> fpStateMachine;

        public Area OffensiveHomeArea { get; set; }
        public List<Area> OffensiveAreas { get; set; }
        public Area DefensiveHomeArea { get; set; }
        public List<Area> DefensiveAreas { get; set; }

        public FieldPlayer(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed,
            Team team = null, Area offHomeArea = null, List<Area> offAreas = null, Area defHomeArea = null, List<Area> defAreas = null, State<FieldPlayer> startState = null) 
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed, team)
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

    }
}
