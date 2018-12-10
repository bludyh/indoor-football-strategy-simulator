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
    abstract class ExtendedUpdateWindow : UpdateWindow {

        private bool isInitialized;
        public event EventHandler Initialized;

        protected override void Initialize() {
            base.Initialize();

            Utilities.SimpleTexture = new Texture2D(GraphicsDevice, 1, 1);
            Utilities.SimpleTexture.SetData(new Color[] { Color.White });
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (Editor != null && !isInitialized) {
                isInitialized = true;
                Initialized?.Invoke(this, new EventArgs());
            }
        }

    }
}
