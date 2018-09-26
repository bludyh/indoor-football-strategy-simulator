using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;

namespace IndoorFootballStrategySimulator {
    class MonoGameWindow : UpdateWindow {

        private float frameRate;

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();
            Editor.spriteBatch.DrawString(Editor.Font, $"fps: { frameRate.ToString("0.0") }", new Vector2(10f, 10f), Color.White);
            Editor.spriteBatch.End();
        }

    }
}
