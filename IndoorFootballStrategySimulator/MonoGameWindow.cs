using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Game;

namespace IndoorFootballStrategySimulator {
    class MonoGameWindow : UpdateWindow {

        public static Texture2D LineTexture { get; private set; }
        public static Random Random { get; private set; }
        public static EntityManager EntityManager { get; private set; }

        static MonoGameWindow() {
            Random = new Random();
            EntityManager = new EntityManager();
        }
        
        protected override void Initialize() {
            base.Initialize();

            LineTexture = new Texture2D(GraphicsDevice, 1, 1);
            LineTexture.SetData(new Color[] { Color.White });

            EntityManager.Initialize(Editor);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            EntityManager.Update(gameTime);
        }

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();
            Editor.spriteBatch.DrawString(Editor.Font, $"Ball velocity: { EntityManager.Ball.Velocity.Length() }", new Vector2(10f, 10f), Color.White);
            EntityManager.Draw(Editor.spriteBatch);
            Editor.spriteBatch.End();
        }

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
