using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    abstract class MovingEntity : Entity {

        public Vector2 Velocity { get; set; }
        public Vector2 Heading { get { return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)); } }
        public Vector2 Side { get { return new Vector2(-Heading.Y, Heading.X); } }
        public float Mass { get; private set; }
        public float MaxForce { get; private set; }
        public float MaxSpeed { get; private set; }

        public MovingEntity(Texture2D texture, Color color, float mass, float maxForce, float maxSpeed) : base(texture, color) {
            Mass = mass;
            MaxForce = maxForce;
            MaxSpeed = maxSpeed;
        }

    }
}
