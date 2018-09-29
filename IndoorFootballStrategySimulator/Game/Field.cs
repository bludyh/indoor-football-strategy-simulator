using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    class Field : Entity {

        public List<Line> Walls { get; private set; }

        public Field(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot) : base(texture, color) {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Walls = new List<Line> {
                new Line(new Vector2(Position.X - Size.X / 2f + 72f, Position.Y - Size.Y / 2f + 8f), new Vector2(Position.X + Size.X / 2f - 72f, Position.Y - Size.Y / 2f + 8f)),
                new Line(new Vector2(Position.X + Size.X / 2f - 72f, Position.Y - Size.Y / 2f + 8f), new Vector2(Position.X + Size.X / 2f - 72f, Position.Y + Size.Y / 2f - 8f)),
                new Line(new Vector2(Position.X + Size.X / 2f - 72f, Position.Y + Size.Y / 2f - 8f), new Vector2(Position.X - Size.X / 2f + 72f, Position.Y + Size.Y / 2f - 8f)),
                new Line(new Vector2(Position.X - Size.X / 2f + 72f, Position.Y + Size.Y / 2f - 8f), new Vector2(Position.X - Size.X / 2f + 72f, Position.Y - Size.Y / 2f + 8f))
            };
        }

        public override void Update(GameTime gameTime) {
            throw new NotImplementedException();
        }

        // Debug code

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            foreach (var line in Walls)
                MonoGameWindow.DrawLine(spriteBatch, line.Start, line.End, Color.Red);
        }
    }
}
