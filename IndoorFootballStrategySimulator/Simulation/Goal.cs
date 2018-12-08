using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    public class Goal : Entity {

        public Vector2 Facing {
            get {
                return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }
        }
        public Vector2 LeftPostPos {
            get {
                return Position + Vector2.Transform(Facing, Matrix.CreateRotationZ(MathHelper.ToRadians(-60f))) * (float)Math.Sqrt(Math.Pow(Size.X / 2f, 2) + Math.Pow(Size.Y / 3f, 2));
            }
        }
        public Vector2 RightPostPos {
            get {
                return Position + Vector2.Transform(Facing, Matrix.CreateRotationZ(MathHelper.ToRadians(60f))) * (float)Math.Sqrt(Math.Pow(Size.X / 2f, 2) + Math.Pow(Size.Y / 3f, 2));
            }
        }
        public Line GoalLine {
            get {
                return new Line(LeftPostPos, RightPostPos);
            }
        }
        public int Score { get; private set; }

        public Goal(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot) : base(texture, color, scale, pos, rot, 0f) { }

        public override void Update(GameTime gameTime) {
            if (IsScored())
                Score++;
        }

        private bool IsScored() {
            Ball ball = SimulationWindow.EntityManager.Ball;

            return GoalLine.Intersect(ball.Position, ball.Radius, out Vector2? intersectionOne, out Vector2? intersectionTwo);
        }

        public void ResetScore() {
            Score = 0;
        }

        // Debug code
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            GoalLine.Draw(spriteBatch, Color.White);
        }

    }
}
