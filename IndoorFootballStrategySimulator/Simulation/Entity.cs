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
    ///     Represents a game object.
    /// </summary>
    /// <remarks>
    ///     This is the base class of every game objects.
    /// </remarks>
    [DataContract]
    public abstract class Entity {

        private Texture2D texture;
        private Color color;

        /// <summary>
        ///     Gets the size of the texture sprite.
        /// </summary>
        public Vector2 TextureSize {
            get {
                return texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height);
            }
        }

        /// <summary>
        ///     Gets the size of the <see cref="Entity"/>.
        /// </summary>
        public Vector2 Size {
            get {
                return TextureSize * Scale;
            }
        }

        /// <summary>
        ///     Gets or sets the scale of the texture sprite.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        ///     Gets or sets the position of the <see cref="Entity"/>.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     Gets or sets the rotation of the <see cref="Entity"/>.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        ///     Gets or sets the bounding circle radius around the <see cref="Entity"/>.
        /// </summary>
        /// <remarks>
        ///     The bouding radius is used for detecting collisions between entities.
        /// </remarks>
        public float Radius { get; set; }

        protected Entity(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius) {
            this.texture = texture;
            this.color = color;
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Position, null, color, Rotation, TextureSize / 2f, Scale, SpriteEffects.None, 0f);
        }

    }
}
