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
            interseciontPoint = null;

            float denom = (other.End.Y - other.Start.Y) * (End.X - Start.X) - (other.End.X - other.Start.X) * (End.Y - Start.Y);
            if (denom == 0) return false;

            float numOne = (other.End.X - other.Start.X) * (Start.Y - other.Start.Y) - (other.End.Y - other.Start.Y) * (Start.X - other.Start.X);
            float numTwo = (End.X - Start.X) * (Start.Y - other.Start.Y) - (End.Y - Start.Y) * (Start.X - other.Start.X);
            float slopeOne = numOne / denom;
            float slopeTwo = numTwo / denom;
            if (slopeOne > 0 && slopeOne < 1 && slopeTwo > 0 && slopeTwo < 1) {
                interseciontPoint = Start + slopeOne * (End - Start);
                return true;
            }

            return false;
        }

        public bool Intersect(Vector2 center, float radius, out Vector2? intersectionOne, out Vector2? intersectionTwo) {
            intersectionOne = null;
            intersectionTwo = null;

            Vector2 d = End - Start;
            Vector2 f = Start - center;
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - radius * radius;
            float discriminant = b * b - 4 * a * c;

            if (discriminant == 0) {
                float t = -b / (2 * a);
                intersectionOne = Start + t * d;
                return true;
            }
            else if (discriminant > 0) {
                float t1 = (float)(-b + Math.Sqrt(discriminant)) / (2 * a);
                float t2 = (float)(-b - Math.Sqrt(discriminant)) / (2 * a);
                intersectionOne = Start + t1 * d;
                intersectionTwo = Start + t2 * d;
                return true;
            }

            return false;
        }

        public float Distance(Vector2 point) {
            float l = Vector2.Distance(Start, End);
            float t = Math.Max(0, Math.Min(1, Vector2.Dot(point - Start, End - Start) / l));
            Vector2 proj = Start + t * (End - Start);
            return Vector2.Distance(point, proj);
        }

    }
}
