using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Contains extension methods.
    /// </summary>
    public static class Extensions {

        /// <summary>
        ///     Truncates a <see cref="Vector2"/> to a maximum length.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="length"></param>
        /// <returns>The result truncated vector.</returns>
        public static Vector2 Truncate(this Vector2 vector, float length) {
            if (vector.Length() > length)
                vector = Vector2.Normalize(vector) * length;
            return vector;
        }

    }
}
