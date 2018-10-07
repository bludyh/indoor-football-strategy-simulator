using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    /// <summary>
    ///     Represents a soccer ball object.
    /// </summary>
    class Ball : MovingEntity {

        private const float Friction = -0.02f;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        /// <param name="texture">texture of the ball.</param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="radius">radius of the bounding circle around the ball.</param>
        /// <param name="mass"></param>
        /// <param name="maxForce"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="walls">list of lines that defines the border of the field.</param>
        public Ball(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, mass, maxForce, maxSpeed) {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
        }

        /// <summary>
        ///     Updates logics of the <see cref="Ball"/>. 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            HandleWallCollisions();

            if (Velocity.Length() > 0 && Velocity.Length() > Friction) {
                Velocity += Vector2.Normalize(Velocity) * Friction;
                Position += Velocity;
            }
        }

        /// <summary>
        ///     Apply a velocity to the <see cref="Ball"/> to simulate kicking action.
        /// </summary>
        /// <param name="direction">a vector indicates the direction of the kick.</param>
        /// <param name="force">power of the kicking force.</param>
        public void Kick(Vector2 direction, float force) {
            direction.Normalize();
            Velocity = (direction * force) / Mass;
        }

        private void HandleWallCollisions () {
            Line closestWall = null;
            float distanceToClosestWall = float.MaxValue;

            foreach (var wall in MonoGameWindow.EntityManager.Field.Walls) {
                if (wall.Intersect(Position, Radius, out Vector2? intersectionOne, out Vector2? intersectionTwo)) {
                    float distanceToWall = wall.Distance(Position);
                    if (distanceToWall < distanceToClosestWall) {
                        distanceToClosestWall = distanceToWall;
                        closestWall = wall;
                    }
                }
            }

            if (closestWall != null && Vector2.Dot(Vector2.Normalize(Velocity), closestWall.Normal) < 0)
                Velocity = Velocity.Reflect(closestWall.Normal) * 0.8f;
        }

    }
}
