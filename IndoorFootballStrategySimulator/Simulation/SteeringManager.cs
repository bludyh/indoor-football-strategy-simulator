﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Controls all the steering behaviors used by players.
    /// </summary>
    public class SteeringManager {

        /// <summary>
        ///     Contains all steering behaviors with their corresponding power-of-two values.
        /// </summary>
        /// <remarks>
        ///     The values are in the form of power-of-two so that bitwise operations can be applied. This serves the purpose of combining different behaviors.
        /// </remarks>
        [Flags]
        public enum SteeringBehavior {
            NONE = 0, SEEK = 1, ARRIVAL = 2, PURSUIT = 4, WALL_AVOIDANCE = 8, INTERPOSE = 16, SEPARATION = 32
        }

        private MovingEntity entity;
        private MovingEntity targetEntity;
        private Vector2 targetPos;
        private SteeringBehavior steeringBehaviors;

        public Vector2 Target { get; set; }

        /// <summary>
        ///     Gets the total steering force.
        /// </summary>
        public Vector2 SteeringForce { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SteeringManager"/> class.
        /// </summary>
        /// <param name="entity">the moving entity that utilizes this steering manager object.</param>
        public SteeringManager(MovingEntity entity) {
            this.entity = entity;
        }

        /// <summary>
        ///     Calculates the force that steers <see cref="entity"/> towards a target position.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns>A vector represents the steering force.</returns>
        private Vector2 Seek(Vector2 targetPos) {
            Vector2 desiredVelocity = Vector2.Normalize(targetPos - entity.Position) * entity.MaxSpeed;
            return desiredVelocity - entity.Velocity;
        }

        /// <summary>
        ///     Start the seek behavior.
        /// </summary>
        /// <param name="targetPos">position to seek.</param>
        /// <remarks>
        ///     This method sets the target position and toggles on the seek bit in <see cref="steeringBehaviors"/>.
        /// </remarks>
        public void StartSeek(Vector2 targetPos) {
            this.targetPos = targetPos;
            steeringBehaviors |= SteeringBehavior.SEEK;
        }

        /// <summary>
        ///     Stop the seek behavior.
        /// </summary>
        /// <remarks>
        ///     This method checks if <see cref="steeringBehaviors"/> already had the seek bit and toggles it off.
        /// </remarks>
        public void StopSeek() {
            if (steeringBehaviors.HasFlag(SteeringBehavior.SEEK))
                steeringBehaviors ^= SteeringBehavior.SEEK;
        }

        /// <summary>
        ///     Calculates the force that slows <see cref="entity"/> down when approaching a target position.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns>A vector represents the steering force.</returns>
        private Vector2 Arrival(Vector2 targetPos) {
            Vector2 targetOffset = targetPos - entity.Position;
            float distance = targetOffset.Length();
            float ramped = entity.MaxSpeed * (distance / 300f);
            float clipped = Math.Min(ramped, entity.MaxSpeed);

            Vector2 desiredVelocity = (clipped / distance) * targetOffset;
            return desiredVelocity - entity.Velocity;
        }

        /// <summary>
        ///     Start the arrival behavior.
        /// </summary>
        /// <param name="targetPos">position to arrive.</param>
        /// <remarks>
        ///     This method sets the target position and toggles on the arrival bit in <see cref="steeringBehaviors"/>.
        /// </remarks>
        public void StartArrival(Vector2 targetPos) {
            this.targetPos = targetPos;
            steeringBehaviors |= SteeringBehavior.ARRIVAL;
        }

        /// <summary>
        ///     Stop the seek behavior.
        /// </summary>
        /// <remarks>
        ///     This method checks if <see cref="steeringBehaviors"/> already had the arrival bit and toggles it off.
        /// </remarks>
        public void StopArrival() {
            if (steeringBehaviors.HasFlag(SteeringBehavior.ARRIVAL))
                steeringBehaviors ^= SteeringBehavior.ARRIVAL;
        }

        /// <summary>
        ///     Calculates the force that steers <see cref="entity"/> towards a predicted position of another <see cref="MovingEntity"/>.
        /// </summary>
        /// <param name="targetEntity">moving entity to pursue.</param>
        /// <returns>A vector represents the steering force.</returns>
        /// <remarks>
        ///     This method is different from <see cref="Seek(Vector2)"/> as it receives a <see cref="MovingEntity"/> object as an argument and
        ///     it calculates the predicted postion of <paramref name="targetEntity"/> after some time and uses seek behavior to steers <see cref="entity"/> to this position.
        ///     The result is a more realistic pursuit behavior.
        /// </remarks>
        private Vector2 Pursuit(MovingEntity targetEntity) {
            Vector2 targetOffset = targetEntity.Position - entity.Position;
            float distance = targetOffset.Length();
            float predictedInterval = distance / (entity.MaxSpeed + targetEntity.Velocity.Length());
            return Seek(targetEntity.Position + targetEntity.Velocity * predictedInterval);
        }

        /// <summary>
        ///     Start the pursuit behavior.
        /// </summary>
        /// <param name="targetEntity">moving entity to pursue.</param>
        /// <remarks>
        ///     This method sets the target entity and toggles on the pursuit bit in <see cref="steeringBehaviors"/>.
        /// </remarks>
        public void StartPursuit(MovingEntity targetEntity) {
            this.targetEntity = targetEntity;
            steeringBehaviors |= SteeringBehavior.PURSUIT;
        }

        /// <summary>
        ///     Stop the pursuit behavior.
        /// </summary>
        /// <remarks>
        ///     This method checks if <see cref="steeringBehaviors"/> already had the pursuit bit and toggles it off.
        /// </remarks>
        public void StopPursuit() {
            if (steeringBehaviors.HasFlag(SteeringBehavior.PURSUIT))
                steeringBehaviors ^= SteeringBehavior.PURSUIT;
        }
        /// <summary>
        ///     Calculates the force that steers <see cref="entity"/> away from any nearby walls.
        /// </summary>
        /// <param name="walls">list of lines that defines the border of the field.</param>
        /// <returns>A vector represents the steering force.</returns>
        /// <remarks>
        ///     This method utilizes the method <see cref="Line.Intersect(Line, out Vector2?)"/> to checks for intersection between <see cref="MovingEntity.WallDetectors"/>
        ///     and <see cref="Field.Walls"/>. Hence, detecting any collisions between <see cref="entity"/> and walls and steering <see cref="entity"/> away. 
        /// </remarks>
        private Vector2 WallAvoidance() {
            Vector2 steeringForce = Vector2.Zero;
            Vector2? intersectionPoint = null;
            Vector2? closestIntersection = null;
            Line closestWall = null;
            float distanceToClosestIntersection = float.MaxValue;

            foreach (var detector in entity.WallDetectors) {
                foreach (var wall in SimulationWindow.EntityManager.Field.Walls) {
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

        /// <summary>
        ///     Start the wall avoidance behavior.
        /// </summary>
        /// <remarks>
        ///     This method toggles on the wall avoidance bit in <see cref="steeringBehaviors"/>.
        /// </remarks>
        public void StartWallAvoidance() {
            steeringBehaviors |= SteeringBehavior.WALL_AVOIDANCE;
        }

        /// <summary>
        ///     Stop the wall avoidance behavior.
        /// </summary>
        /// <remarks>
        ///     This method checks if <see cref="steeringBehaviors"/> already had the wall avoidance bit and toggles it off.
        /// </remarks>
        public void StopWallAvoidance() {
            if (steeringBehaviors.HasFlag(SteeringBehavior.WALL_AVOIDANCE))
                steeringBehaviors ^= SteeringBehavior.WALL_AVOIDANCE;
        }

        /// <summary>
        ///     Calculates the force that steers <see cref="entity"/> towards a position between <paramref name="targetEntity"/> and <paramref name="otherEntity"/>.
        /// </summary>
        /// <param name="movingEntity">target entity.</param>
        /// <param name="target">another entity moving towards target entity.</param>
        /// <returns>A vector represents the steering force.</returns>
        /// <remarks>
        ///     This method can be applied, for example, when a player wants to intercept the ball from another player.
        /// </remarks>
        private Vector2 Interpose(MovingEntity movingEntity, Vector2 target)
        {
            return Arrival(Vector2.Add(target, Vector2.Multiply(Vector2.Normalize(Vector2.Subtract(movingEntity.Position, target)),
                    20f)));
        }

        /// <summary>
        ///     Start the interpose behavior.
        /// </summary>
        /// <param name="targetEntity"></param>
        /// <param name="target"></param>
        /// <remarks>
        ///     This method sets the target entity and other entity and toggles on the interpose bit in <see cref="steeringBehaviors"/>.
        /// </remarks>
        public void StartInterpose(MovingEntity targetEntity, Vector2 target) {
            this.targetEntity = targetEntity;
            this.targetPos = target;
            steeringBehaviors |= SteeringBehavior.INTERPOSE;
        }

        /// <summary>
        ///     Stop the interpose behavior.
        /// </summary>
        /// <remarks>
        ///     This method checks if <see cref="steeringBehaviors"/> already had the interpose bit and toggles it off.
        /// </remarks>
        public void StopInterpose() {
            if (steeringBehaviors.HasFlag(SteeringBehavior.INTERPOSE))
                steeringBehaviors ^= SteeringBehavior.INTERPOSE;
        }

        /// <summary>
        ///     Calculates the force that repels <see cref="entity"/> from nearby players.
        /// </summary>
        /// <returns>A vector represents the steering force.</returns>
        private Vector2 Separation() {
            Vector2 steeringForce = new Vector2();
            foreach (var entity in SimulationWindow.EntityManager.Entities)
            {
                if (entity is Player player && player != this.entity)
                {
                    Vector2 offset = Vector2.Subtract(player.Position, this.entity.Position);
                    Vector2.Add(steeringForce, Vector2.Divide(Vector2.Normalize(offset), offset.Length()));;
                }
            }
            return steeringForce;

        }

        /// <summary>
        ///     Start the separation behavior.
        /// </summary>
        /// <remarks>
        ///     This method toggles on the separation bit in <see cref="steeringBehaviors"/>.
        /// </remarks>
        public void StartSeparation() {
            steeringBehaviors |= SteeringBehavior.SEPARATION;
        }

        /// <summary>
        ///     Stop the separation behavior.
        /// </summary>
        /// <remarks>
        ///     This method checks if <see cref="steeringBehaviors"/> already had the separation bit and toggles it off.
        /// </remarks>
        public void StopSeparation() {
            if (steeringBehaviors.HasFlag(SteeringBehavior.SEPARATION))
                steeringBehaviors ^= SteeringBehavior.SEPARATION;
        }

        /// <summary>
        ///     Calculates the total steering force by summing up all forces from active behaviors.
        /// </summary>
        /// <returns>A vector represents the total steering force.</returns>
        public Vector2 Calculate() {
            SteeringForce = Vector2.Zero;
            Vector2 force = Vector2.Zero;

            if (steeringBehaviors.HasFlag(SteeringBehavior.WALL_AVOIDANCE)) {
                force += WallAvoidance() * 10f;
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehaviors.HasFlag(SteeringBehavior.SEPARATION)) {
                force += Separation()*20f;
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehaviors.HasFlag(SteeringBehavior.SEEK)) {
                force += Seek(targetPos);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehaviors.HasFlag(SteeringBehavior.ARRIVAL)) {
                force += Arrival(targetPos);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehaviors.HasFlag(SteeringBehavior.PURSUIT)) {
                force += Pursuit(targetEntity);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            if (steeringBehaviors.HasFlag(SteeringBehavior.INTERPOSE)) {
                force += Interpose(targetEntity, targetPos);
                if (!AccumulateSteeringForce(force)) return SteeringForce;
            }

            return SteeringForce;
        }

        /// <summary>
        ///     Checks if the added force exceeds the maximum force and adjust the total steering force accordingly.
        /// </summary>
        /// <param name="force">added force.</param>
        /// <returns>A boolean indicates whether the added force exceeds the maximum force.</returns>
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
