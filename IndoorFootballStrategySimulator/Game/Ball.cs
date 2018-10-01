using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    class Ball : MovingEntity {

        private const float Friction = -0.015f;
        private readonly List<Line> walls;

        public Ball(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed, List<Line> walls)
            : base(texture, color, mass, maxForce, maxSpeed) {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
            this.walls = walls;
        }

        public override void Update(GameTime gameTime) {
            HandleWallCollisions();
            if (Velocity.Length() > 0 && Velocity.Length() > Friction) {
                Velocity += Vector2.Normalize(Velocity) * Friction;
                Position += Velocity;
            }
        }

        public void Kick(Vector2 direction, float force) {
            direction.Normalize();
            Velocity = (direction * force) / Mass;
        }

        private void HandleWallCollisions () {
            Line closestWall = null;
            float distanceToClosestWall = float.MaxValue;

            foreach (var wall in walls) {
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

=======

namespace IndoorFootballStrategySimulator.Game
{
    class Ball
    {
>>>>>>> Added Class for AI
    }
}
