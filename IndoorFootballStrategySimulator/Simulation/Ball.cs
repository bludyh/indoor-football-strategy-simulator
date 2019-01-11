using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a soccer ball object.
    /// </summary>
    public class Ball : MovingEntity
    {

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
        public Ball(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed) { }

        /// <summary>
        ///     Updates logics of the <see cref="Ball"/>. 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            HandleWallCollisions();

            if (Velocity.Length() > 0 && Velocity.Length() > Friction)
            {
                Velocity += Vector2.Normalize(Velocity) * Friction;
                Position += Velocity;
            }
        }

        /// <summary>
        ///     Apply a velocity to the <see cref="Ball"/> to simulate kicking action.
        /// </summary>
        /// <param name="direction">a vector indicates the direction of the kick.</param>
        /// <param name="force">power of the kicking force.</param>
        public void Kick(Vector2 direction, float force)
        {
            direction.Normalize();
            Velocity = (direction * force) / Mass;
        }

        /// <summary>
        ///     Checks if the <see cref="Ball"/> collides with any walls and repels it back into the <see cref="Field"/>.
        /// </summary>
        /// <seealso cref="Line.Intersect(Vector2, float, out Vector2?, out Vector2?)"/>
        private void HandleWallCollisions()
        {
            Line closestWall = null;
            float distanceToClosestWall = float.MaxValue;

            foreach (var wall in SimulationWindow.EntityManager.Field.Walls)
            {
                if (wall.Intersect(Position, Radius, out Vector2? intersectionOne, out Vector2? intersectionTwo))
                {
                    float distanceToWall = wall.Distance(Position);
                    if (distanceToWall < distanceToClosestWall)
                    {
                        distanceToClosestWall = distanceToWall;
                        closestWall = wall;
                    }
                }
            }

            if (closestWall != null && Vector2.Dot(Vector2.Normalize(Velocity), closestWall.Normal) < 0)
                Velocity = Vector2.Reflect(Velocity, closestWall.Normal) * 0.8f;
        }


        public void Trap()
        {
            Velocity = Vector2.Zero;
        }

        public double TimeToCoverDistance(Vector2 A,Vector2 B, float force)
        {
            //this will be the velocity of the ball in the next time step *if*
            //the player was to make the pass. 
            float speed = force / Mass;

            //calculate the velocity at B using the equation
            //
            //  v^2 = u^2 + 2as
            //

            //first calculate s (the distance between the two positions)
            float DistanceToCover = Vector2.Distance(A, B);

            float term = (float)(speed * speed + 2.0 * DistanceToCover * Friction);

            //if  (u^2 + 2as) is negative it means the ball cannot reach point B.
            if (term <= 0.0)
            {
                return -1.0;
            }

            float v = (float)Math.Sqrt(term);

            //it IS possible for the ball to reach B and we know its speed when it
            //gets there, so now it's easy to calculate the time using the equation
            //
            //    t = v-u
            //        ---
            //         a
            //
            return (v - speed) / Friction;
        }

        public override bool HandleMessage(Telegram msg)
        {
            return false;
        }
    }
}
