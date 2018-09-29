using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IndoorFootballStrategySimulator.Game {
    class Line {

        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }
        public Vector2 Normal {
            get {
                Vector2 direction = Vector2.Normalize(End - Start);
                return new Vector2(-direction.Y, direction.X);
            }
        }

        public Line(Vector2 start, Vector2 end) {
            Start = start;
            End = end;
        }

        public bool Intersect(Line other, out Vector2? interseciontPoint) {
            float denom = (other.End.Y - other.Start.Y) * (End.X - Start.X) - (other.End.X - other.Start.X) * (End.Y - Start.Y);

            if (denom == 0) {
                interseciontPoint = null;
                return false;
            }

            float numOne = (other.End.X - other.Start.X) * (Start.Y - other.Start.Y) - (other.End.Y - other.Start.Y) * (Start.X - other.Start.X);
            float numTwo = (End.X - Start.X) * (Start.Y - other.Start.Y) - (End.Y - Start.Y) * (Start.X - other.Start.X);
            float slopeOne = numOne / denom;
            float slopeTwo = numTwo / denom;

            if (slopeOne > 0 && slopeOne < 1 && slopeTwo > 0 && slopeTwo < 1) {
                interseciontPoint = Start + slopeOne * (End - Start);
                return true;
            }

            interseciontPoint = null;
            return false;
        }

    }
}
