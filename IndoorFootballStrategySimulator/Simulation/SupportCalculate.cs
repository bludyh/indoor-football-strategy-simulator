using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    class SupportCalculate
    {
        //private class for Spots
        private class SupportSpot
        {
            public Vector2 Position;
            public double Score;
            public SupportSpot(Vector2 position, double value)
            {
                Position = position;
                Score = value;
            }
        }
        private static List<SupportSpot> SpotsList = new List<SupportSpot>();
        //a pointer to the highest valued spot from the last update
        private static SupportSpot bestSupportingSpot;
        public static Team Team { get; private set; }
        public SupportCalculate(int numX, int numY, Team team)
        {
            bestSupportingSpot = null;
            Team = team;
            Area playingArea = SimulationWindow.EntityManager.Field.PlayingArea;
            //calculate the positions of each sweet spot, create them and 
            //store them in m_Spots
            float HeightOfSSRegion = playingArea.Height * 0.8f;
            float WidthOfSSRegion = playingArea.Width * 0.9f;
            float SliceX = WidthOfSSRegion / numX;
            float SliceY = HeightOfSSRegion / numY;

            float left = playingArea.LeftX + (playingArea.Width - WidthOfSSRegion) / 2.0f + SliceX / 2.0f;
            float right = playingArea.RightX - (playingArea.Width - WidthOfSSRegion) / 2.0f - SliceX / 2.0f;
            float top = playingArea.TopY + (playingArea.Height - HeightOfSSRegion) / 2.0f + SliceY / 2.0f;

            for (int x = 0; x < (numX / 2) - 1; ++x)
            {
                for (int y = 0; y < numY; ++y)
                {
                    if (Team.Color == TeamColor.BLUE)
                    {
                        SpotsList.Add(new SupportSpot(new Vector2(left + x * SliceX, top + y * SliceY), 0.0));
                    }

                    else
                    {
                        SpotsList.Add(new SupportSpot(new Vector2(right - x * SliceX, top + y * SliceY), 0.0));
                    }
                }
            }

        }
        /// <summary>
        /// Returns +1 if value is clockwise of this vector, -1 if anticlockwise (Y axis pointing down, X axis to right)
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Sign(Vector2 vector1, Vector2 vector2)
        {
            if (vector1.Y * vector2.X > vector1.X * vector2.Y)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Transforms a vector by a matrix
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public static void TransformVector2(Matrix param, Vector2 value)
        {
            value.X = (param.M11 * value.X) + (param.M21 * value.Y) + param.M31;
            value.Y = (param.M12 * value.X) + (param.M22 * value.Y) + param.M32;
        }

        /// <summary>
        /// Returns the vector perpendicular to the param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Vector2 Perpendicular(Vector2 param)
        {
            return new Vector2(-param.Y, param.X);
        }
        public static Vector2 PointToLocalSpace(Vector2 point, Vector2 AgentHeading, Vector2 AgentSide, Vector2 AgentPosition)
        {
            //make a copy of the point
            Vector2 TransPoint = point;

            //create a transformation matrix
            Matrix matTransform = new Matrix();

            float Tx = -Vector2.Dot(AgentPosition, AgentHeading);
            float Ty = -Vector2.Dot(AgentPosition, AgentSide);

            //create the transformation matrix
            matTransform.M11 = AgentHeading.X; matTransform.M12 = AgentSide.X;
            matTransform.M21 = AgentHeading.Y; matTransform.M22 = AgentSide.Y;
            matTransform.M31 = Tx; matTransform.M32 = Ty;

            //now transform the vertices
            TransformVector2(matTransform, TransPoint);

            return TransPoint;
        }
        public static Vector2 GetBestSupportingSpot()
        {
            if (bestSupportingSpot != null)
            {
                return bestSupportingSpot.Position;
            }
            else
            {
                return DetermineBestSupportingPosition();
            }
        }
        public static Vector2 DetermineBestSupportingPosition()
        {                             
            if ( bestSupportingSpot != null)
            {
                return bestSupportingSpot.Position;
            }
            //reset the best supporting spot
            bestSupportingSpot = null;
            double BestScoreSoFar = 0;
            foreach (SupportSpot spot in SpotsList)
            {
                spot.Score = 1;
                if (Team.isPassSafeFromAllOpponents(Team.ControllingPlayer.Position,spot.Position,null, 3f))
                {
                    spot.Score += 2;
                }
                //Test 2. Determine if a goal can be scored from this position.  
                if (Team.CanShoot(spot.Position,6f))
                {
                    spot.Score += 1.0;
                }
                //Test 3. calculate how far this spot is away from the controlling
                //player. The further away, the higher the score. Any distances further
                //away than OptimalDistance pixels do not receive a score.
                if (Team.SupportingPlayer != null)
                {
                    const float OptimalDistance = 200f;

                    float dist = Vector2.Distance(Team.ControllingPlayer.Position, spot.Position);

                    float temp = Math.Abs(OptimalDistance - dist);

                    if (temp < OptimalDistance)
                    {
                        //normalize the distance and add it to the score
                        spot.Score += 2.0* (OptimalDistance - temp) / OptimalDistance;
                    }
                }

                //check to see if this spot has the highest score so far
                if (spot.Score > BestScoreSoFar)
                {
                    BestScoreSoFar = spot.Score;

                    bestSupportingSpot = spot;
                }
            }
            return bestSupportingSpot.Position;
        }
        // ---- GetTangentPoints method
         //   Given a point P and a circle of radius R centered at C this function
         // determines the two points on the circle that intersect with the 
         //  tangents from P to the circle. Returns false if P is within the circle.
        public static bool GetTangentPoints(Vector2 C, float R, Vector2 P, Vector2 T1, Vector2 T2)
        {
            Vector2 PmC = Vector2.Subtract(P, C);
            float SqrLen = PmC.LengthSquared();
            float RSqr = R * R;
            if (SqrLen <= RSqr)
            {
                // P is inside or on the circle
                return false;
            }

            float InvSqrLen = 1 / SqrLen;
            float Root = (float)Math.Sqrt(Math.Abs(SqrLen - RSqr));

            T1.X = C.X + R * (R * PmC.X - PmC.Y * Root) * InvSqrLen;
            T1.Y = C.Y + R * (R * PmC.Y + PmC.X * Root) * InvSqrLen;
            T2.X = C.X + R * (R * PmC.X + PmC.Y * Root) * InvSqrLen;
            T2.Y = C.Y + R * (R * PmC.Y - PmC.X * Root) * InvSqrLen;

            return true;
        }
        public static float RandFloat()
        {

            return (float)new Random().NextDouble();
        }
    }
}
