using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a soccer field object.
    /// </summary>
    public class Field : Entity {

        /// <summary>
        ///     Gets a list of <see cref="Line"/> that defines the border of the <see cref="Field"/>.
        /// </summary>
        public List<Line> Walls { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="texture">texture of the field.</param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
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

        public override void Update(GameTime gameTime) { }

        // Debug code
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            foreach (var line in Walls)
                SimulationWindow.DrawLine(spriteBatch, line.Start, line.End, Color.Red);
        }
    }
}
