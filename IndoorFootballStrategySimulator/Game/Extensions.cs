using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    static class Extensions {

        public static Vector2 Truncate(this Vector2 vector, float length) {
            if (vector.Length() > length)
                vector = Vector2.Normalize(vector) * length;
            return vector;
        }

        public static Vector2 Reflect(this Vector2 vector, Vector2 normal) {
            return vector - 2f * Vector2.Dot(vector, normal) * normal;
        }

    }
}
