using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a player.
    /// </summary>
    public class Player : MovingEntity {

        /// <summary>
        ///     Gets the <see cref="SteeringManager"/> of the current <see cref="Player"/>.
        /// </summary>
        public SteeringManager Steering { get; private set; }
        public Team Team { get; private set; }
        public Field Field { get; private set; }
        public float DistanceToBall { get; private set; }

        
        /// <summary>
        ///     Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        /// <param name="maxForce"></param>
        /// <param name="maxSpeed"></param>
        public Player(Team team, Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, mass, maxForce, maxSpeed) {
            Team = team;
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
            Steering = new SteeringManager(this);
        }

        /// <summary>
        ///     Updates logics of the <see cref="Player"/>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            HandleOverlapping();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 force = Steering.Calculate();
            Vector2 acceleration = force / Mass;
            Velocity += acceleration * deltaTime;
            Velocity = Velocity.Truncate(MaxSpeed);
            Position += Velocity * deltaTime;
            if (Velocity.Length() > 1f)
                Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
        }

        /// <summary>
        ///     Pulls every surrounding entities away if the bounding circles are overlapped.
        /// </summary>
        /// <remarks>
        ///     This method only checks again other moving entities whose radius of the bounding circle is greater than zero.
        /// </remarks>
        private void HandleOverlapping() {
            foreach (var entity in SimulationWindow.EntityManager.Entities) {
                if (entity is MovingEntity movingEntity && movingEntity != this) {
                    Vector2 offset = movingEntity.Position - Position;
                    float overlap = Radius + movingEntity.Radius - offset.Length();
                    if (overlap >= 0)
                        movingEntity.Velocity += Vector2.Normalize(offset) * overlap;
                }
            }
        }
        /// <summary>
        /// Return true if the player is the closet player in his team to the ball
        /// </summary>
        /// <returns></returns>
        public Boolean isClosetTeamMemberToBall() {
            if (Team.PlayerClosetToBall() == this)
                return true;
            return false;
        }

        public Boolean isClosetPlayerOnPitchToBall() {
            // return isClosetPlayerOnPitchToBall() && (DistanceToBall < Team.Opponents().ClosetDisToBall());
            return false;
        }

        public Boolean BallWithinKeeperRange() {
            //TODO 
            // return (Vector2.Distance(Position,Ball))
            return false;
        }
       
        // Debug code
        //public override void Draw(SpriteBatch spriteBatch) {
        //    base.Draw(spriteBatch);

        //    MonoGameWindow.DrawLine(spriteBatch, Position, Position + Steering.SteeringForce, Color.Red);
        //}

    }
}