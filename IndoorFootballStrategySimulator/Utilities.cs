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
    static class Utilities {

        public static Random Random { get; private set; }
        public static Texture2D SimpleTexture { get; set; }

        static Utilities() {
            Random = new Random();
        }

    }
}
