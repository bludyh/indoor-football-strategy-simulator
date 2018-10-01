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
            NONE = 0, SEEK = 1, ARRIVAL = 2, PURSUIT = 4, WALL_AVOIDANCE = 8
        }

        private MovingEntity entity;
        private MovingEntity targetEntity;
        private Vector2 targetPos;
        private List<Line> walls;
        private SteeringBehavior steeringBehavior;

        public Vector2 SteeringForce { get; private set; }

        public SteeringManager(MovingEntity entity) {
            this.entity = entity;
        }

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

        private Vector2 Pursuit(MovingEntity targetEntity) {
            Vector2 targetOffset = targetEntity.Position - entity.Position;
            float distance = targetOffset.Length();
            float predictedInterval = distance / (entity.MaxSpeed + targetEntity.Velocity.Length());
            return Seek(targetEntity.Position + targetEntity.Velocity * predictedInterval);
        }

        public void StartPursuit(MovingEntity targetEntity) {
            this.targetEntity = targetEntity;
            steeringBehavior |= SteeringBehavior.PURSUIT;
        }

        public void StopPursuit() {
            if (steeringBehavior.HasFlag(SteeringBehavior.PURSUIT))
                steeringBehavior ^= SteeringBehavior.PURSUIT;
        }

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

        public Vector2 Calculate() {
            SteeringForce = Vector2.Zero;
            Vector2 force = Vector2.Zero;

            if (steeringBehavior.HasFlag(SteeringBehavior.WALL_AVOIDANCE)) {
                force += WallAvoidance(walls) * 10f;
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

    }
}
