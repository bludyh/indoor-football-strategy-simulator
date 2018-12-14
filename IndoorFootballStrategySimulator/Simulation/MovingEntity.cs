using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a moving entity.
    /// </summary>
    [DataContract]
    public abstract class MovingEntity : Entity {

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
        public Vector2 Side { get { return SupportCalculate.Perpendicular(Heading); } }

        /// <summary>
        ///     Gets a list of <see cref="Line"/> that represents wall detectors.
        /// </summary>
        /// <remarks>
        ///     The wall detectors are used for detecting collisions with walls.
        /// </remarks>
        public List<Line> WallDetectors
        {
            get
            {
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
        public float MaxSpeed { get; set; }

        protected MovingEntity(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, scale, pos, rot, radius)
        {
            Mass = mass;
            MaxForce = maxForce;
            MaxSpeed = maxSpeed;
        }
        public bool RotateHeadingToFacePosition(Vector2 target)
        {
            Vector2 toTarget = Vector2.Normalize(Vector2.Subtract(target, this.Position));

            //first determine the angle between the heading vector and the target
            float angle = (float)Math.Acos(Vector2.Dot(this.Heading, toTarget));
            //return true if the player is facing the target
            if (angle < 0.00001f)
            {
                return true;
            }
            //The next few lines use a rotation matrix to rotate the player's heading
            //vector accordingly
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle * SupportCalculate.Sign(Heading,toTarget));
            SupportCalculate.TransformVector2(rotationMatrix, Heading);
            SupportCalculate.TransformVector2(rotationMatrix, Velocity);
            return false;
        }
        public abstract bool HandleMessage(Telegram msg);
    }
}
