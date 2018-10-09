using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    class SteeringManager {

        [Flags]
        public enum SteeringBehavior {
            NONE = 0, SEEK = 1, ARRIVAL = 2, PURSUIT = 4, WALL_AVOIDANCE = 8, SEPARATION = 10, INTERPOSE = 12,
        }
        // List player
        private List<Player> listplayers;
        private MovingEntity entity;
        private MovingEntity targetEntity;
        private Vector2 targetPos;
        private List<Line> walls;
        private SteeringBehavior steeringBehavior;

        private double interposeDist;

        public Vector2 SteeringForce { get; private set; }

        public SteeringManager(MovingEntity entity) {
            this.entity = entity;
        }

        // SEEK
        private Vector2 Seek(Vector2 targetPos) {
            Vector2 desiredVelocity = Vector2.Normalize(targetPos - entity.Position) * entity.MaxSpeed;
            return desiredVelocity - entity.Velocity;
        }

        public void StartSeek() {
            steeringBehavior |= SteeringBehavior.SEEK;
        }

        public void StopSeek() {
            if (steeringBehavior.HasFlag(SteeringBehavior.SEEK))
                steeringBehavior ^= SteeringBehavior.SEEK;
        }

        //ARRIVAL
        private Vector2 Arrival(Vector2 targetPos) {
            Vector2 targetOffset = targetPos - entity.Position;
            float distance = targetOffset.Length();
            float ramped = entity.MaxSpeed * (distance / 300f);
            float clipped = Math.Min(ramped, entity.MaxSpeed);

            Vector2 desiredVelocity = (clipped / distance) * targetOffset;
            return desiredVelocity - entity.Velocity;
        }

        public void StartArrival(Vector2 targetPos) {
            this.targetPos = targetPos;
            steeringBehavior |= SteeringBehavior.ARRIVAL;
        }

        public void StopArrival() {
            if (steeringBehavior.HasFlag(SteeringBehavior.ARRIVAL))
                steeringBehavior ^= SteeringBehavior.ARRIVAL;
        }

        //PURSUIT
        private Vector2 Pursuit(MovingEntity targetEntity) {
            Vector2 targetOffset = targetEntity.Position - entity.Position;
            float distance = targetOffset.Length();
            float predictedInterval = distance / (entity.MaxSpeed + targetEntity.Velocity.Length());
            return Arrival(targetEntity.Position + targetEntity.Velocity * predictedInterval);
        }

        public void StartPursuit(MovingEntity targetEntity) {
            this.targetEntity = targetEntity;
            steeringBehavior |= SteeringBehavior.PURSUIT;
        }

        public void StopPursuit() {
            if (steeringBehavior.HasFlag(SteeringBehavior.PURSUIT))
                steeringBehavior ^= SteeringBehavior.PURSUIT;
        }

        //WALLAVOIDANCE
        private Vector2 WallAvoidance(List<Line> walls) {
            Vector2 steeringForce = Vector2.Zero;
            Vector2? intersectionPoint = null;
            Vector2? closestIntersection = null;
            Line closestWall = null;
            float distanceToClosestIntersection = float.MaxValue;

            foreach (var detector in entity.WallDetectors) {
                foreach (var wall in walls) {
                    if (detector.Intersect(wall, out intersectionPoint)) {
                        float distanceToIntersection = Vector2.Distance(entity.Position, intersectionPoint.Value);
                        if (distanceToIntersection < distanceToClosestIntersection) {
                            distanceToClosestIntersection = distanceToIntersection;
                            closestIntersection = intersectionPoint;
                            closestWall = wall;
                        }
                    }
                }
                if (closestWall != null) {
                    Vector2 excessiveVelocity = detector.End - closestIntersection.Value;
                    steeringForce = closestWall.Normal * excessiveVelocity.Length();
                }
            }
            return steeringForce;
        }

        public void StartWallAvoidance(List<Line> walls) {
            this.walls = walls;
            steeringBehavior |= SteeringBehavior.WALL_AVOIDANCE;
        }

        public void StopWallAvoidance() {
            if (steeringBehavior.HasFlag(SteeringBehavior.WALL_AVOIDANCE))
                steeringBehavior ^= SteeringBehavior.WALL_AVOIDANCE;
        }

        // CALCULATE
        public Vector2 Calculate() {
            SteeringForce = Vector2.Zero;
            Vector2 force = Vector2.Zero;
            if (steeringBehavior.HasFlag(SteeringBehavior.SEPARATION))
            {
                force += Separation(listplayers);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }
            if (steeringBehavior.HasFlag(SteeringBehavior.WALL_AVOIDANCE)) {
                //force += WallAvoidance(walls) * 10f;
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehavior.HasFlag(SteeringBehavior.SEEK)) {
                force += Seek(targetPos);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehavior.HasFlag(SteeringBehavior.ARRIVAL)) {
                force += Arrival(targetPos);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehavior.HasFlag(SteeringBehavior.PURSUIT)) {
                force += Pursuit(targetEntity);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            return SteeringForce;
        }

        private bool AccumulateSteeringForce(Vector2 force) {
            if (force.Length() > 0) {
                float remainingForceMagnitude = entity.MaxForce - SteeringForce.Length();
                if (remainingForceMagnitude <= 0f) return false;

                float forceMagnitude = force.Length();
                if (forceMagnitude > remainingForceMagnitude)
                    forceMagnitude = remainingForceMagnitude;

                SteeringForce += Vector2.Normalize(force) * forceMagnitude;
            }
            return true;
        }


        // SEPARATION
        public Vector2 Separation(List<Player> listPlayers)
        {
            Vector2 steeringforce = Vector2.Zero;
            Vector2 distance = Vector2.Zero;
            int j = 0;
            for (int i = 0; i < listPlayers.Count; i++)
            {
                Player curPlayer = listPlayers.ElementAt(i);
                distance = Vector2.Subtract(entity.Position, curPlayer.Position);
                if (curPlayer != entity && distance.Length()<= entity.Radius)
                {
                    steeringforce += Vector2.Subtract(curPlayer.Position, entity.Position);
                    j++;
                }
            }
            if (j > 0)
            {
                steeringforce = Vector2.Divide(steeringforce, j);
                steeringforce = Vector2.Negate(steeringforce);
                steeringforce = Vector2.Normalize(steeringforce);
                Console.WriteLine(steeringforce);
                return steeringforce;
            }
            else
            {
                Console.WriteLine(steeringforce);
                return steeringforce;
            }

        }

        public void StartSeparation(List<Player> listPlayers)
        {
            this.listplayers = listPlayers;
            steeringBehavior |= SteeringBehavior.SEPARATION;
        }

        public void StopSeparation()
        {
            if (steeringBehavior.HasFlag(SteeringBehavior.SEPARATION))
                steeringBehavior ^= SteeringBehavior.SEPARATION;
        }

        //INTERPOSE

        public Vector2 Interpose(Ball ball) {

            //Determine the midpoint between goalkeeper and the ball
            Vector2 MidPoint = (ball.Position + entity.Position) / 2;

            double TimeToReachMidPoint = Vector2.Distance(entity.Position, MidPoint) / entity.MaxSpeed;
            Vector2 ballPos = ball.Position + Vector2.Multiply(ball.Velocity,(float)TimeToReachMidPoint);
            Vector2 entityPos = entity.Position + Vector2.Multiply(entity.Velocity, (float)TimeToReachMidPoint);
            // predicted position of midpoint
          return  MidPoint = (ballPos + entityPos) / 2;
          //  return Arrival(MidPoint);
        }

        public void InterposeOn(double d) {
            interposeDist = d;
            steeringBehavior |= SteeringBehavior.INTERPOSE;
        }
        /// <summary>
        /// Set Target Position
        /// </summary>
        /// <param name="t"></param>
        public void SetTarget(Vector2 t) {
            targetPos = t;
        }
    }
}
