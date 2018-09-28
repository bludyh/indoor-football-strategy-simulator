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

        private float frameRate;
        private Player player;

        protected override void Initialize() {
            base.Initialize();

            Texture2D texture = Editor.Content.Load<Texture2D>("characterBlue (1)");
            player = new Player(texture, Color.White, new Vector2(1f, 1f), new Vector2(100f, 100f), 0f, 0f, 1f, 100f, 100f);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.Steering.StartSeek(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            player.Update(gameTime);
        }

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();
            Editor.spriteBatch.DrawString(Editor.Font, $"fps: { frameRate.ToString("0.0") }", new Vector2(10f, 10f), Color.White);
            player.Draw(Editor.spriteBatch);
            Editor.spriteBatch.End();
        }

    }
}
