using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    abstract class Player : MovingEntity {

        public SteeringManager Steering { get; private set; }

        public Player(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, mass, maxForce, maxSpeed) {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
            Steering = new SteeringManager(this);
        }

        public override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 force = Steering.Calculate();
            Vector2 acceleration = force / Mass;
            Velocity += acceleration * deltaTime;
            Velocity.Truncate(MaxSpeed);
            Position += Velocity * deltaTime;
            if (Velocity.Length() > 1f)
                Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            MonoGameWindow.DrawLine(spriteBatch, Position, Position + Steering.SteeringForce, Color.Red);
        }

    }
}