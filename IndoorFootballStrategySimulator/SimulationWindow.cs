using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Simulation;

namespace IndoorFootballStrategySimulator {
    public class SimulationWindow : UpdateWindow {

        public static Texture2D LineTexture { get; private set; }

        /// <summary>
        ///     Gets the manager that controls all entities.
        /// </summary>
        public static EntityManager EntityManager { get; private set; }

        /// <summary>
        ///     Initializes custom contents.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();

            LineTexture = new Texture2D(GraphicsDevice, 1, 1);
            LineTexture.SetData(new Color[] { Color.White });
            EntityManager = new EntityManager();
            EntityManager.Initialize(Editor);
        }

        /// <summary>
        ///     Main update method.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        ///     This method is the main game loop that gets called multiple time per second.
        /// </remarks>
        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            EntityManager.Update(gameTime);
        }

        /// <summary>
        ///     Main draw method.
        /// </summary>
        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();
            EntityManager.Draw(Editor.spriteBatch);
            Editor.spriteBatch.End();
        }

        /// <summary>
        ///     Draws a <see cref="Line"/> between two points.
        /// </summary>
        /// <param name="sb">spritebatch object.</param>
        /// <param name="start">starting point.</param>
        /// <param name="end">ending point.</param>
        /// <param name="color">color of the line.</param>
        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color) {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(LineTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

    }
}
