﻿using Microsoft.Xna.Framework;
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
        private SupportCalculate supportCalculate;
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
            supportCalculate = new SupportCalculate(13, 6, this);
            ClosestDistancetoBall = 0;
            teamStateMachine = new FSM<Team>(this);
            SupportingPlayer = null;
            ReceivingPlayer = null;
            ControllingPlayer = null;
            PlayerClosestToBall = null;
        }
        private void Initialize(UpdateService editor)
        {
            switch (Color) {
                case TeamColor.BLUE:
                    Goal = new Goal(editor.Content.Load<Texture2D>($"SoccerGoal"), Microsoft.Xna.Framework.Color.White, new Vector2(1f), new Vector2(40f, 288f), 0f);
                    SimulationWindow.EntityManager.Entities.Add(Goal);
                    break;
                case TeamColor.RED:
                    Goal = new Goal(editor.Content.Load<Texture2D>($"SoccerGoal"), Microsoft.Xna.Framework.Color.White, new Vector2(1f), new Vector2(1240f, 288f), MathHelper.Pi);
                    SimulationWindow.EntityManager.Entities.Add(Goal);
                    break;
            }
        }

        private void SetStrategy(Strategy strategy) {
            this.strategy = strategy;
            SimulationWindow.EntityManager.Entities.RemoveAll(e => e is Player p && p.Team == this);
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

                player.Steering.StartWallAvoidance();
                SimulationWindow.EntityManager.Entities.Add(player);
            }
        }

        //Update
        public void Update(GameTime gameTime)
        {
            if (teamStateMachine.CurrentState == null)
            {
                teamStateMachine.SetCurrentState(Defensive.Instance());
                teamStateMachine.SetGlobalState(null);
            }

            CalculateClosestPlayerToBall();
            teamStateMachine.Update(gameTime);
        }
        
        //Get FSM
        public FSM<Team> GetFSM()
        {
            return teamStateMachine;
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
                if ((player.Role == PlayerRole.Attacker) && (player != ControllingPlayer) && ControllingPlayer != null)
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
            Vector2 target = new Vector2();
            bool finded = false;
            //iterate through all this player's team members and calculate which
            //one is in a position to be passed the ball
            foreach (Player curPlayer in Strategy.Players)
            {
                if ((curPlayer != passer) && (Vector2.DistanceSquared(passer.Position, curPlayer.Position) > MinPassingDistance * MinPassingDistance))
                {
                    if (GetBestPassToReceiver(passer, curPlayer, target, power))
                    {
                        //if the pass target is the closest to the opponent's goal line found
                        // so far, keep a record of it
                        float Dist2Goal = Math.Abs(target.X - Opponent.Goal.Center.X);

                        if (Dist2Goal < ClosestToGoalSoFar)
                        {
                            ClosestToGoalSoFar = Dist2Goal;

                            //keep a record of this player
                            receiver = curPlayer;
                            //and the target
                            PassTarget = target;
                            finded = true;
                        }
                    }
                }
            }
            return finded;
        }

        public bool IsOpponentWithinRadius(Vector2 position, float radius)
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
                if (player.InHomeArea() == false)
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

        
        public void RequestPass(FieldPlayer requester)
        {
            if(SupportCalculate.RandFloat()>0.1)
            {
                return;
            }
            if (IsPassSafeFromAllOpponents(ControllingPlayer.Position, requester.Position, requester, 3f))
            {

                //tell the player to make the pass
                //let the receiver know a pass is coming 
                MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MESSAGE_IMMEDIATELY,
                    requester,
                    ControllingPlayer,
                    MessageTypes.Msg_PassToMe,
                    requester);

            }
        }
        public void ReturnAllPlayersToHome()
        {
            GoalKeeper goalKeeper = (GoalKeeper)Strategy.Players.Find(x => x is GoalKeeper);
            goalKeeper.GetFSM().ChangeState(ReturnHome.Instance());
            foreach (Player player in Strategy.Players)
            {
                if (player.Role != PlayerRole.GoalKeeper)
                {
                    MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MESSAGE_IMMEDIATELY,
                        goalKeeper,
                        player,
                        MessageTypes.Msg_GoHome,
                        null);
                }
            }

        }
        
        public void UpdateTargetsOfWaitingPlayers()
        {
            foreach (Player curPlayer in Strategy.Players)
            {
                if (curPlayer.Role != PlayerRole.GoalKeeper)
                {
                    FieldPlayer player = (FieldPlayer)curPlayer;
                    if (player.GetFSM().IsInState(Idle.Instance()) || player.GetFSM().IsInState(ReturnToHomeArea.Instance()))
                    {
                        player.Steering.Target = player.GetHomeArea(SimulationWindow.EntityManager.Field,State).Center;
                    }
                }
            }
        }

        public bool IsPassSafeFromOpponent(Vector2 from, Vector2 target, Player receiver, Player opp, float PassingForce)
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
            if (Vector2.DistanceSquared(from, target) < Vector2.DistanceSquared(opp.Position, from))
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
            //calculate how long it takes the ball to cover the distance to the 
            //position orthogonal to the opponents position
            double TimeForBall = SimulationWindow.EntityManager.Ball.TimeToCoverDistance(new Vector2(0, 0), new Vector2(LocalPosOpp.X, 0), PassingForce);

            //now calculate how far the opponent can run in this time
            double reach = opp.MaxSpeed * TimeForBall
                    + SimulationWindow.EntityManager.Ball.Radius
                    + opp.Radius;

            //if the distance to the opponent's y position is less than his running
            //range plus the radius of the ball and the opponents radius then the
            //ball can be intercepted
            if (Math.Abs(LocalPosOpp.Y) < reach)
            {
                return false;
            }

            return true;
        }

        public bool GetBestPassToReceiver(Player passer, Player receiver, Vector2 PassTarget, float power)
        {
            var Ball = SimulationWindow.EntityManager.Ball;
            //first, calculate how much time it will take for the ball to reach 
            //this receiver, if the receiver was to remain motionless 
            double time = Ball.TimeToCoverDistance(Ball.Position, receiver.Position, power);

            //return false if ball cannot reach the receiver after having been
            //kicked with the given power
            if (time < 0)
            {
                return false;
            }

            //the maximum distance the receiver can cover in this time
            float InterceptRange = (float)time * receiver.MaxSpeed;

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
                        && IsPassSafeFromAllOpponents(SimulationWindow.EntityManager.Ball.Position,
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
                float minValue = Opponent.Goal.LeftPostPos.Y , maxValue = Opponent.Goal.RightPostPos.Y;
                if (minValue >= maxValue )
                {
                    minValue = Opponent.Goal.RightPostPos.Y;
                    maxValue = Opponent.Goal.LeftPostPos.Y;
                }
                float MinYVal = (minValue+ SimulationWindow.EntityManager.Ball.Radius);
                float MaxYVal =(maxValue - SimulationWindow.EntityManager.Ball.Radius);
                shotTarget.Y = new Random().NextFloat(MinYVal, MaxYVal);
                //make sure striking the ball with the given power is enough to drive
                //the ball over the goal line.
                double time = SimulationWindow.EntityManager.Ball.TimeToCoverDistance(position,shotTarget,power);
                if (time>=0)
                {
                    if (IsPassSafeFromAllOpponents(position, shotTarget, null, power))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

       //Need check
        public bool IsPassSafeFromAllOpponents(Vector2 from, Vector2 target, Player receiver, float PassingForce)
        {
            foreach (Player player in Opponent.Strategy.Players)
            {
                if (!IsPassSafeFromOpponent(from, target, receiver, player, PassingForce))
                {
                    return false;
                }
            }
            return true;
        }

    }
}

