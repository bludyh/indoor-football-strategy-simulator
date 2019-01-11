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
    public class GoalKeeper : Player
    {

        private FSM<GoalKeeper> gkStateMachine;
        private State<GoalKeeper> startState;

        [DataMember]
        public int HomeArea { get; set; }

        [DataMember]
        public List<int> Areas { get; set; }

        public GoalKeeper(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed,
            TeamColor team = TeamColor.BLUE, int homeArea = -1, List<int> areas = null, State<GoalKeeper> startState = null)
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed, team)
        {
            HomeArea = homeArea;
            Areas = areas;
            gkStateMachine = new FSM<GoalKeeper>(this);
            this.startState = startState;
        }

        public override Area GetHomeArea(Field field, TeamState state) {
            return field.Areas[HomeArea];
        }

        public override List<Area> GetAreas(Field field, TeamState state) {
            var areas = new List<Area>();

            foreach (var area in Areas) {
                areas.Add(field.Areas[area]);
            }

            return areas;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gkStateMachine.CurrentState == null && startState != null)
            {
                gkStateMachine.SetCurrentState(startState);
                gkStateMachine.SetGlobalState(GlobalGoalKeeperState.Instance());
            }

            gkStateMachine.Update(gameTime);
        }
        public FSM<GoalKeeper> GetFSM()
        {
            return gkStateMachine;
        }

        /// <summary>
        /// when the ball becomes within this distance of the goalkeeper
        /// changes state to intercept the ball
        /// Goal keeper intercept range =100
        /// </summary>
        /// <returns></returns>
        public bool TooFarFromGoalMouth() {
            return (Vector2.DistanceSquared(Position, GetRearInterposeTarget()) > (100f*100f));
        }

        public Vector2 GetRearInterposeTarget() {
            var field = SimulationWindow.EntityManager.Field;
            var ball = SimulationWindow.EntityManager.Ball;

            float xPosTarget = Team.Goal.Center.X;

            float yPosTarget = field.PlayingArea.Center.Y- Vector2.Distance(Team.Goal.LeftPostPos,Team.Goal.RightPostPos)*0.5f
                +(ball.Position.Y * Vector2.Distance(Team.Goal.LeftPostPos, Team.Goal.RightPostPos)) / field.PlayingArea.Height;

            return new Vector2(xPosTarget,yPosTarget);
        }

        public bool BallWithinRangeForIntercept() {
            return (Vector2.DistanceSquared(Team.Goal.Center, SimulationWindow.EntityManager.Ball.Position) < (100f *100f));
        }

        public override bool HandleMessage(Telegram msg)
        {
            return gkStateMachine.HandleMessage(msg);
        }
    }
}

