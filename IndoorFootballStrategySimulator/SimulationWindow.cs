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
    class SimulationWindow : ExtendedUpdateWindow {
		
        /// <summary>
        ///     Gets the manager that controls all entities.
        /// </summary>
        public static EntityManager EntityManager { get; private set; }

        static SimulationWindow() {
            EntityManager = new EntityManager();
        }

        /// <summary>
        ///     Initializes custom contents.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();

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
            if (!Simulator.Pause)
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

    }
}
