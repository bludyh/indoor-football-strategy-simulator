using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Describes a 2D line segment with 2 endpoints and a normal.
    /// </summary>
    public class Line {

        /// <summary>
        ///     Gets the starting point of the <see cref="Line"/>.
        /// </summary>
        public Vector2 Start { get; private set; }

        /// <summary>
        ///     Gets the ending point of the <see cref="Line"/>.
        /// </summary>
        public Vector2 End { get; private set; }

        /// <summary>
        ///     Gets the normal of the <see cref="Line"/>.
        /// </summary>
        public Vector2 Normal {
            get {
                Vector2 direction = Vector2.Normalize(End - Start);
                return new Vector2(-direction.Y, direction.X);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="start">starting point of the line.</param>
        /// <param name="end">ending point of the line.</param>
        public Line(Vector2 start, Vector2 end) {
            Start = start;
            End = end;
        }

        /// <summary>
        ///     Checks for intersection between two lines. The <paramref name="interseciontPoint"/> param returns the point of intersection.
        /// </summary>
        /// <param name="other">other line to check for intersection.</param>
        /// <param name="interseciontPoint">point of intersection.</param>
        /// <returns>A boolean indicates whether the two lines intersect.</returns>
        /// <remarks>
        ///     <paramref name="interseciontPoint"/> returns null if there is no intersection.
        /// </remarks>
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

        /// <summary>
        ///     Checks for intersection between a <see cref="Line"/> and a circle. The <paramref name="intersectionOne"/> and <paramref name="intersectionTwo"/> params return the points of intersection.
        /// </summary>
        /// <param name="center">center point of the circle.</param>
        /// <param name="radius">radius of the circle.</param>
        /// <param name="intersectionOne">point of intersection.</param>
        /// <param name="intersectionTwo">point of intersection.</param>
        /// <returns>A boolean indicates whether the line intersects the circle.</returns>
        /// <remarks>
        ///     <para><paramref name="intersectionOne"/> and <paramref name="intersectionTwo"/> both return null if there is no intersection.</para>
        ///     <para>Only <paramref name="intersectionOne"/> has a value if the line is a tangent to the circle. Hence, there is one intersection point.</para>
        ///     <para><paramref name="intersectionOne"/> and <paramref name="intersectionTwo"/> both have values if the line intersects the circle at two unique points.</para>
        /// </remarks>
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

        /// <summary>
        ///     Calculates the distance between a point to the <see cref="Line"/>.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>A float represents the distance.</returns>
        public float Distance(Vector2 point) {
            float l = Vector2.Distance(Start, End);
            float t = Math.Max(0, Math.Min(1, Vector2.Dot(point - Start, End - Start) / l));
            Vector2 proj = Start + t * (End - Start);
            return Vector2.Distance(point, proj);
        }

    }
}
