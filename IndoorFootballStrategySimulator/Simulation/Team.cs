using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class Team
    {
        private readonly FSM<Team> teamStateMachine;
        private Strategy strategy;

        public Player PlayerClosestToBall { get; set; }
        public Player ControllingPlayer { get; set; }
        public Player ReceivingPlayer { get; set; }
        public Player SupportingPlayer { get; set; }
        public Goal Goal { get; private set; }
        public TeamColor Color { get; private set; }
        public TeamState State { get; set; }
        public float ClosestDistancetoBall { get; set; }

        public Team Opponent {
            get {
                switch (Color) {
                    case TeamColor.BLUE:
                        return SimulationWindow.EntityManager.RedTeam;
                    case TeamColor.RED:
                        return SimulationWindow.EntityManager.BlueTeam;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Strategy Strategy {
            get { return strategy; }
            set => SetStrategy(value);
        }

        public Team(UpdateService editor, TeamColor color)
        {
            Color = color;

            Initialize(editor);

            teamStateMachine = new FSM<Team>(this);
            teamStateMachine.SetCurrentState(Defensive.Instance());
        }
        private void Initialize(UpdateService editor)
        {
            switch (Color) {
                case TeamColor.BLUE:
                    Goal = new Goal(editor.Content.Load<Texture2D>($"SoccerGoal"), Microsoft.Xna.Framework.Color.White, new Vector2(1f), new Vector2(40f, 288f), 0f);
                    SimulationWindow.EntityManager.Entities.Add(Goal);
                    break;
                case TeamColor.RED:
                    Goal = new Goal(editor.Content.Load<Texture2D>($"SoccerGoal"), Microsoft.Xna.Framework.Color.White, new Vector2(1f, 1f), new Vector2(1240f, 288f), MathHelper.Pi);
                    SimulationWindow.EntityManager.Entities.Add(Goal);
                    break;
            }
        }

        private void SetStrategy(Strategy strategy) {
            this.strategy = strategy;

            for (int i = 0; i < Strategy.Players.Count; i++) {
                var player = Strategy.Players[i];

                switch (Color) {
                    case TeamColor.BLUE:
                        player.Position = SimulationWindow.EntityManager.Field.HomeTeamSpawnAreas[i].Center;
                        break;
                    case TeamColor.RED:
                        player.Position = SimulationWindow.EntityManager.Field.AwayTeamSpawnAreas[i].Center;
                        break;
                }

                SimulationWindow.EntityManager.Entities.Add(player);
            }
        }

        //Update
        public void Update(GameTime gameTime)
        {
            CalculateClosestPlayerToBall();
            teamStateMachine.Update(gameTime);
        }
        //Get FSM
        public FSM<Team> GetFSM()
        {
            return teamStateMachine;
        }
        private void Behaviors()
        {
            foreach (var player in Strategy.Players)
            {
                player.Steering.StartWallAvoidance();
            }
        }
        private void CalculateClosestPlayerToBall()
        {
            float ClosestSoFar = float.MaxValue;

            foreach (Player curPlayer in Strategy.Players)
            {
                float dist = Vector2.DistanceSquared(curPlayer.Position, SimulationWindow.EntityManager.Ball.Position);
                curPlayer.DistanceToBall = dist;
                if (dist < ClosestSoFar)
                {
                    ClosestSoFar = dist;
                    PlayerClosestToBall = curPlayer;
                }
            }

            ClosestDistancetoBall = ClosestSoFar;
        }
        public bool CanShoot(Vector2 BallPos, float power)
        {
            return CanShoot(BallPos, power, new Vector2());
        }
        public bool InControl()
        {
            if (ControllingPlayer != null)
                return true;
            return false;
        }
        public Player DetermineBestSupportingAttacker() {
            float ClosestSoFar = float.MaxValue;

            Player BestPlayer = null;

            foreach (Player player in Strategy.Players)
            {
                if ((player.PlayerRole == PlayerRole.Attacker) && (player != ControllingPlayer))
                {
                    float dist = Vector2.DistanceSquared(player.Position, SupportCalculate.GetBestSupportingSpot());
                    if (dist < ClosestSoFar)
                    {
                        ClosestSoFar = dist;
                        BestPlayer = player;
                    }
                }
            }

            return BestPlayer;
        }
        public bool FindPass(Player passer, Player receiver, Vector2 PassTarget, float power, float MinPassingDistance)
        {
            float ClosestToGoalSoFar = float.MaxValue;
            Vector2 Target = new Vector2();
            bool finded = false;
            //iterate through all this player's team members and calculate which
            //one is in a position to be passed the ball
            foreach (Player curPlayer in Strategy.Players)
            {
                if ((curPlayer != passer) && (Vector2.DistanceSquared(passer.Position, curPlayer.Position) > MinPassingDistance * MinPassingDistance))
                {
                    if (GetBestPassToReceiver(passer, curPlayer, Target, power))
                    {
                        //if the pass target is the closest to the opponent's goal line found
                        // so far, keep a record of it
                        float Dist2Goal = Math.Abs(Target.X - Opponent.Goal.Center.X);

                        if (Dist2Goal < ClosestToGoalSoFar)
                        {
                            ClosestToGoalSoFar = Dist2Goal;

                            //keep a record of this player
                            receiver = curPlayer;

                            //and the target
                            PassTarget = Target;

                            finded = true;
                        }
                    }
                }
            }
            return finded;
        }
        public bool isOpponentWithinRadius(Vector2 position, float radius)
        {
            foreach (Player player in Opponent.Strategy.Players)
            {
                if (Vector2.DistanceSquared(position, player.Position) < radius * radius)
                {
                    return true;
                }
            }
            return false;
        }
        public bool AllPlayersAtHome()
        {
            foreach (Player player in Strategy.Players)
            {
                if (player.InHomeRegion() == false)
                {
                    return false;
                }
            }
            return true;
        }
        public void SetControllingPlayer(Player player)
        {
            ControllingPlayer = player;
            //rub it in the opponents faces!
            Opponent.LostControl();
        }
        public void LostControl()
        {
            ControllingPlayer = null;
        }

        #region TODO
        //Missing Event
        public void RequestPass(FieldPlayer requester)
        {
            if(SupportCalculate.RandFloat()>0.1f)
            {
                return;
            }
            if (isPassSafeFromAllOpponents(ControllingPlayer.Position, requester.Position, requester, 3f))
            {

                //tell the player to make the pass
                //let the receiver know a pass is coming 

            }
        }
        public void ReturnAllPlayersToHome()
        {
            //TODO

        }
        //Miss HomeArea
        public void UpdateTargetsOfWaitingPlayers()
        {
            foreach (Player curPlayer in Strategy.Players)
            {
                if (curPlayer.PlayerRole != PlayerRole.GoalKeeper)
                {
                    FieldPlayer player = (FieldPlayer)curPlayer;
                    if (player.GetFSM().IsInState(Idle.Instance()) || player.GetFSM().IsInState(ReturnToHomeRegion.Instance()))
                    {
                        //player.Steering.Target = player.HomeArea.Center;
                    }
                }
            }
        }
        #endregion

        #region TOCHECK
        // No ball timecovertodistance method
        public bool isPassSafeFromOpponent(Vector2 from, Vector2 target, Player receiver, Player opp, float PassingForce)
        {
            //move the opponent into local space.
            Vector2 ToTarget = Vector2.Subtract(target, from);
            Vector2 ToTargetNormalized = Vector2.Normalize(ToTarget);

            Vector2 LocalPosOpp = SupportCalculate.PointToLocalSpace(opp.Position, ToTargetNormalized, SupportCalculate.Perpendicular(ToTargetNormalized), from);

            //if opponent is behind the kicker then pass is considered okay(this is 
            //based on the assumption that the ball is going to be kicked with a 
            //velocity greater than the opponent's max velocity)
            if (LocalPosOpp.X < 0)
            {
                return true;
            }

            //if the opponent is further away than the target we need to consider if
            //the opponent can reach the position before the receiver.
            if (Vector2.Distance(from, target) < Vector2.Distance(opp.Position, from))
            {
                if (receiver != null)
                {
                    if (Vector2.DistanceSquared(target, opp.Position)
                            > Vector2.DistanceSquared(target, receiver.Position))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return true;
                }
            }
            //if the distance to the opponent's y position is less than his running
            //range plus the radius of the ball and the opponents radius then the
            //ball can be intercepted
            if (Math.Abs(LocalPosOpp.Y) < SimulationWindow.EntityManager.Ball.Position.Y)
            {
                return false;
            }

            return true;
        }
        public bool GetBestPassToReceiver(Player passer, Player receiver, Vector2 PassTarget, float power)
        {
            float InterceptRange = receiver.MaxSpeed;

            //Scale the intercept range
            float ScalingFactor = 0.3f;
            InterceptRange *= ScalingFactor;

            //now calculate the pass targets which are positioned at the intercepts
            //of the tangents from the ball to the receiver's range circle.
            Vector2 ip1 = new Vector2(), ip2 = new Vector2();

            SupportCalculate.GetTangentPoints(receiver.Position,InterceptRange, SimulationWindow.EntityManager.Ball.Position, ip1,ip2);

            Vector2[] Passes = { ip1, receiver.Position, ip2 };
            int NumPassesToTry = Passes.Length;

            // this pass is the best found so far if it is:
            //
            //  1. Further upfield than the closest valid pass for this receiver
            //     found so far
            //  2. Within the playing area
            //  3. Cannot be intercepted by any opponents

            float ClosestSoFar = float.MaxValue;
            bool bResult = false;

            for (int pass = 0; pass < NumPassesToTry; ++pass)
            {
                float dist = Math.Abs(Passes[pass].X - Opponent.Goal.Center.X);

                if ((dist < ClosestSoFar)
                        && SimulationWindow.EntityManager.Field.PlayingArea.Inside(Passes[pass])
                        && isPassSafeFromAllOpponents(SimulationWindow.EntityManager.Ball.Position,
                        Passes[pass],
                        receiver,
                        power))
                {
                    ClosestSoFar = dist;
                    PassTarget = Passes[pass];
                    bResult = true;
                }
            }

            return bResult;
        }
        public bool CanShoot(Vector2 position, float power, Vector2 shotTarget)
        {
            //the number of randomly created shot targets this method will test 
            int NumAttempts = 5;

            while (NumAttempts-- > 0)
            {
                //choose a random position along the opponent's goal mouth. (making
                //sure the ball's radius is taken into account)
                shotTarget = Opponent.Goal.Center;

                //the y value of the shot position should lay somewhere between two
                //goalposts (taking into consideration the ball diameter)
                int MinYVal = (int)(Opponent.Goal.LeftPostPos.Y + SimulationWindow.EntityManager.Ball.Radius);
                int MaxYVal = (int)(Opponent.Goal.RightPostPos.Y - SimulationWindow.EntityManager.Ball.Radius);

                Random rand = new Random();
                shotTarget.Y = rand.Next(MinYVal, MaxYVal);
                if (isPassSafeFromAllOpponents(position, shotTarget, null, power))
                {
                    return true;
                }
            }

            return false;
        }
        //Missing event
        public bool isPassSafeFromAllOpponents(Vector2 from, Vector2 target, Player receiver, float PassingForce)
        {
            foreach (Player player in Opponent.Strategy.Players)
            {
                if (!isPassSafeFromOpponent(from, target, receiver, player, PassingForce))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}

