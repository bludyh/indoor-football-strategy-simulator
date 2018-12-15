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
        private State<FieldPlayer> startState;

        [DataMember]
        public int OffensiveHomeArea { get; set; }

        [DataMember]
        public List<int> OffensiveAreas { get; set; }

        [DataMember]
        public int DefensiveHomeArea { get; set; }

        [DataMember]
        public List<int> DefensiveAreas { get; set; }

        public FieldPlayer(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed,
            TeamColor team = TeamColor.BLUE, PlayerRole role= PlayerRole.Attacker, int offHomeArea = -1, List<int> offAreas = null, int defHomeArea = -1, List<int> defAreas = null, State<FieldPlayer> startState = null) 
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed, team, role)
        {
            OffensiveHomeArea = offHomeArea;
            OffensiveAreas = offAreas;
            DefensiveHomeArea = defHomeArea;
            DefensiveAreas = defAreas;
            fpStateMachine = new FSM<FieldPlayer>(this);
            this.startState = startState;
        }

        public override Area GetHomeArea(Field field, TeamState state) {
            switch (state) {
                case TeamState.OFFENSIVE:
                    return field.Areas[OffensiveHomeArea];
                case TeamState.DEFENSIVE:
                    return field.Areas[DefensiveHomeArea];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override List<Area> GetAreas(Field field, TeamState state) {
            var areas = new List<Area>();

            switch (state) {
                case TeamState.OFFENSIVE:
                    foreach (var area in OffensiveAreas)
                        areas.Add(field.Areas[area]);
                    break;
                case TeamState.DEFENSIVE:
                    foreach (var area in DefensiveAreas)
                        areas.Add(field.Areas[area]);
                    break;
            }

            return areas;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fpStateMachine.CurrentState == null && startState != null)
            {
                fpStateMachine.SetCurrentState(startState);
                fpStateMachine.SetGlobalState(GlobalFieldPlayerState.Instance());
            }
            fpStateMachine.Update(gameTime);
        }
        public FSM<FieldPlayer> GetFSM()
        {
            return fpStateMachine;
        }

        public override bool HandleMessage(Telegram msg)
        {
            return fpStateMachine.HandleMessage(msg);
        }
    }
}
