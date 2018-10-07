using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    /// <summary>
    ///     Represents a moving entity.
    /// </summary>
    abstract class MovingEntity : Entity {

        /// <summary>
        ///     Gets or sets the velocity of the <see cref="MovingEntity"/>.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        ///     Gets the heading vector of the <see cref="MovingEntity"/>.
        /// </summary>
        public Vector2 Heading { get { return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)); } }

        /// <summary>
        ///     Gets the side vector of the <see cref="MovingEntity"/>.
        /// </summary>
        public Vector2 Side { get { return new Vector2(-Heading.Y, Heading.X); } }

        /// <summary>
        ///     Gets a list of <see cref="Line"/> that represents wall detectors.
        /// </summary>
        /// <remarks>
        ///     The wall detectors are used for detecting collisions with walls.
        /// </remarks>
        public List<Line> WallDetectors {
            get {
                return new List<Line> {
                    new Line(Position, Position + Heading * 30f),
                    new Line(Position, Position + Vector2.Transform(Heading, Matrix.CreateRotationZ(MathHelper.ToRadians(-45f))) * 30f),
                    new Line(Position, Position + Vector2.Transform(Heading, Matrix.CreateRotationZ(MathHelper.ToRadians(45f))) * 30f)
                };
            }
        }

        /// <summary>
        ///     Gets the mass of the <see cref="MovingEntity"/>.
        /// </summary>
        public float Mass { get; private set; }

        /// <summary>
        ///     Gets the maximum force produced by the <see cref="MovingEntity"/>.
        /// </summary>
        public float MaxForce { get; private set; }

        /// <summary>
        ///     Gets the maximum speed of the <see cref="MovingEntity"/>.
        /// </summary>
        public float MaxSpeed { get; private set; }

        protected MovingEntity(Texture2D texture, Color color, float mass, float maxForce, float maxSpeed) : base(texture, color) {
            Mass = mass;
            MaxForce = maxForce;
            MaxSpeed = maxSpeed;
        }

        // Debug code
        //public override void Draw(SpriteBatch spriteBatch) {
        //    base.Draw(spriteBatch);

        //    MonoGameWindow.DrawLine(spriteBatch, Position, Position + Velocity, Color.Blue);
        //}

    }
}
