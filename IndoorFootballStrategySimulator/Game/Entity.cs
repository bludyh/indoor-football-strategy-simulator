using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    abstract class Entity {

        private Texture2D texture;
        private Color color;

        public Vector2 Size { get { return texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height); } }
        public Vector2 Scale { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
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
