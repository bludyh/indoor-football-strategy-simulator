using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a player.
    /// </summary>
    [DataContract]
    [KnownType(typeof(GoalKeeper))]
    [KnownType(typeof(FieldPlayer))]
    public class Player : MovingEntity {

        /// <summary>
        ///     Gets the <see cref="SteeringManager"/> of the current <see cref="Player"/>.
        /// </summary>
        public SteeringManager Steering { get; private set; }
        public Team Team { get; private set; }
        public Field Field { get { return SimulationWindow.EntityManager.Field; }}
        public Ball Ball { get { return SimulationWindow.EntityManager.Ball; }}
        public float DistanceToBall { get; set; }
        public PlayerRole PlayerRole { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        /// <param name="maxForce"></param>
        /// <param name="maxSpeed"></param>
        public Player(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed, Team team, PlayerRole role)
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed) {
            Team = team;
            PlayerRole = role;
            Steering = new SteeringManager(this);
        }

        /// <summary>
        ///     Updates logics of the <see cref="Player"/>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            HandleOverlapping();
          
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 force = Steering.Calculate();
            Vector2 acceleration = force / Mass;
            Velocity += acceleration * deltaTime;
            Velocity = Velocity.Truncate(MaxSpeed);
            Position += Velocity * deltaTime;
            if (Velocity.Length() > 1f)
                Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
        }

        /// <summary>
        ///     Pulls every surrounding entities away if the bounding circles are overlapped.
        /// </summary>
        /// <remarks>
        ///     This method only checks again other moving entities whose radius of the bounding circle is greater than zero.
        /// </remarks>
        private void HandleOverlapping() {
            foreach (var entity in SimulationWindow.EntityManager.Entities) {
                if (entity is MovingEntity movingEntity && movingEntity != this) {
                    Vector2 offset = movingEntity.Position - Position;
                    float overlap = Radius + movingEntity.Radius - offset.Length();
                    if (overlap >= 0)
                        movingEntity.Velocity += Vector2.Normalize(offset) * overlap;
                }
            }
        }
        /// <summary>
        /// Return true if the player is the closet player in his team to the ball
        /// </summary>
        /// <returns></returns>
        public bool BallWithinKeeperRange() {
            return (Vector2.DistanceSquared(this.Position, Ball.Position) < (10f * 10f));
        }
        public bool BallWithinKickingRange()
        {
            return (Vector2.DistanceSquared(Ball.Position, this.Position) < (10f*10f));
        }
        public bool BallWithinReceivingRange()
        {
            return (Vector2.DistanceSquared(this.Position, Ball.Position) < 10f*10f);
        }
        public bool isThreatened()
        {
            //check against all opponents to make sure non are within this
            //player's comfort zone
            foreach (Player oppPlayer in Team.Opponent.Strategy.Players)
            {
                if (PositionInFrontOfPlayer(oppPlayer.Position) && (Vector2.DistanceSquared(this.Position,oppPlayer.Position)<(60f*60f)))
                {
                    return true;
                }
            }
            return false;
        }
        public void TrackBall()
        {
            RotateHeadingToFacePosition(Ball.Position);
        }
        public bool isAheadOfAttacker()
        {
            return (Math.Abs(this.Position.X - Team.Opponent.Goal.Center.X)
                    < Math.Abs(Team.ControllingPlayer.Position.X - Team.Opponent.Goal.Center.X));
        }
        public bool AtTarget()
        {
            return (Vector2.DistanceSquared(this.Position, this.Steering.Target) < (10f*10f));
        }
        public bool isClosestTeamMemberToBall() {
            if (Team.PlayerClosestToBall == this)
                return true;
            return false;
        }
        public bool PositionInFrontOfPlayer(Vector2 position)
        {
            Vector2 ToSubject = Vector2.Subtract(position, this.Position);

            if (Vector2.Dot(ToSubject,this.Heading) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isClosestPlayerOnPitchToBall() {
            if (isClosestTeamMemberToBall() && DistanceToBall < Team.Opponent.ClosestDistancetoBall)
            {
                return true;
            }
            return false;
        }
        public bool isControllingPlayer()
        {
            if (Team.ControllingPlayer == this)
            {
                return true;
            }
            return false;
        }
        public bool isControllingPlayer()
        {
            if (Team.ControllingPlayer == this)
            {
                return true;
            }
            return false;
        }
        public bool InHotRegion()
        {
            return Math.Abs(Position.X - Team.OpponentsGoal.Center.X) < Field.PlayingArea.Length / 3f;
        }
        protected Area GetPlayerHomeArea(Player player)
        {
            if (player is GoalKeeper goalKeeper)
                return Field.Areas[goalKeeper.HomeArea];
            else if (player is FieldPlayer fieldPlayer)
            {
                switch (Team.TeamState)
                {
                    case TeamState.OFFENSIVE:
                        return Field.Areas[fieldPlayer.OffensiveHomeArea];
                    case TeamState.DEFENSIVE:
                        return Field.Areas[fieldPlayer.DefensiveHomeArea];
                }
            }
            return null;
        }
        #region TODO
        public bool InHomeRegion()
        {
            if (PlayerRole == PlayerRole.GoalKeeper)
            {
                return HomeRegion.Inside(Position,Area.AreaModifer.Normal);
            }
            else
            {
                return HomeRegion.Inside(Position,Area.AreaModifer.HalfSize);
            }
        }
        //Missing Event
        public void FindSupport()
        {
            //if there is no support we need to find a suitable player.
            if (Team.SupportingPlayer == null)
            {
                Player bestSupportPlayer = Team.DetermineBestSupportingAttacker();
                Team.SupportingPlayer = bestSupportPlayer;
            }

            Player BestSupportPlayer = Team.DetermineBestSupportingAttacker();

            //if the best player available to support the attacker changes, update
            //the pointers and send messages to the relevant players to update their
            //states
            if (BestSupportPlayer != null && (BestSupportPlayer != Team.SupportingPlayer))
            {
                Team.SupportingPlayer = BestSupportPlayer;
            }
        }
        #endregion
    }
}