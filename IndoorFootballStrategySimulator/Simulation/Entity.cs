using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a game object.
    /// </summary>
    /// <remarks>
    ///     This is the base class of every game objects.
    /// </remarks>
    public abstract class Entity {

        private Texture2D texture;
        private Color color;

        /// <summary>
        ///     Return the width and height of the texture sprite as a <see cref="Vector2"/>.
        /// </summary>
        public Vector2 Size { get { return texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height); } }

        /// <summary>
        ///     Get or set the scale of the texture sprite.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        ///     Get or set the position of the <see cref="Entity"/>.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     Get or set the rotation of the <see cref="Entity"/>.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        ///     Get or set the bounding circle radius around the <see cref="Entity"/>.
        /// </summary>
        /// <remarks>
        ///     The bouding radius is used for detecting collisions between entities.
        /// </remarks>
        public float Radius { get; set; }

        protected Entity(Texture2D texture, Color color) {
            this.texture = texture;
            this.color = color;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Position, null, color, Rotation, Size / 2f, Scale, SpriteEffects.None, 0f);
        }

    }
}
