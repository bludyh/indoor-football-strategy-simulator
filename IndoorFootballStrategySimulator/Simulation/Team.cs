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
        //team color
        public enum Color
        {
            RED, BLUE
        }
        public List<Player> listMembers = new List<Player>();
        public Player PlayerClosestToBall { get; set; }
        public Player ControllingPlayer { get; set; }
        public Player ReceivingPlayer { get; set; }
        public Player SupportingPlayer { get; set; }
        public Team Opponents { get; private set; }
        private readonly FSM<Team> teamStateMachine;
        public Color TeamColor { get; private set; }
        public Goal Goal { get; private set; }
        public Goal HomeGoal { get; private set; }
        public Goal OpponentsGoal { get; private set; }
        public float ClosestDistancetoBall { get; set; }

        public Team(Goal homeGoal, Goal opponentsGoal, Color color, UpdateService editor)
        {
            this.TeamColor = color;
            HomeGoal = homeGoal;
            OpponentsGoal = opponentsGoal;
            CreatePlayers(editor);
            Behaviors();
            teamStateMachine = new FSM<Team>(this);
            teamStateMachine.SetCurrentState(Defensive.Instance());
            PlayerClosestToBall = ControllingPlayer = ReceivingPlayer = SupportingPlayer = null;
            ClosestDistancetoBall = 0f;
        }
        private void CreatePlayers(UpdateService editor)
        {

            if (TeamColor == Color.BLUE)
            {
                //Draw Blue Team
                Texture2D texture = editor.Content.Load<Texture2D>($"CharacterBlue-{ Utilities.Random.Next(1, 6) }");
                //Goal Keeper
                GoalKeeper GK = new GoalKeeper(
                    texture,
                    Microsoft.Xna.Framework.Color.White,
                    new Vector2(1f, 1f),
                    new Vector2(80f, 288f),
                    0f,
                    15f,
                    3f,
                    75f,
                    50f,
                    this,
                    startState: TendGoal.Instance());
                SimulationWindow.EntityManager.Entities.Add(GK);
                SimulationWindow.EntityManager.Players.Add(GK);
                listMembers.Add(GK);
                //Field Players
                for (int i = 0; i < 4; i++)
                {
                    texture = editor.Content.Load<Texture2D>($"CharacterBlue-{ Utilities.Random.Next(1, 6) }");
                    FieldPlayer FP = new FieldPlayer(
                        texture,
                        Microsoft.Xna.Framework.Color.White,
                        new Vector2(1f, 1f),
                        new Vector2(Utilities.Random.Next(80, 640), Utilities.Random.Next(30, 546)),
                        0f,
                        15f,
                        3f,
                        75f,
                        Utilities.Random.Next(30, 50),
                        this,
                        startState: Idle.Instance());
                    SimulationWindow.EntityManager.Entities.Add(FP);
                    SimulationWindow.EntityManager.Players.Add(FP);
                    listMembers.Add(FP);
                }
            }
            else
            {
                //Draw Red Team
                Texture2D texture = editor.Content.Load<Texture2D>($"CharacterRed-{ Utilities.Random.Next(1, 6) }");
                // Goal Keeper
                GoalKeeper GK = new GoalKeeper(
                    texture,
                    Microsoft.Xna.Framework.Color.White,
                    new Vector2(1f, 1f),
                    new Vector2(1200f, 288f),
                    MathHelper.Pi,
                    15f,
                    3f,
                    75f,
                    50f,
                    this,
                    startState: TendGoal.Instance());
                SimulationWindow.EntityManager.Entities.Add(GK);
                SimulationWindow.EntityManager.Players.Add(GK);
                listMembers.Add(GK);
                //Field Players
                for (int i = 0; i < 4; i++)
                {
                    texture = editor.Content.Load<Texture2D>($"CharacterRed-{ Utilities.Random.Next(1, 6) }");
                    FieldPlayer FP = new FieldPlayer(
                        texture,
                        Microsoft.Xna.Framework.Color.White,
                        new Vector2(1f, 1f),
                        new Vector2(Utilities.Random.Next(640, 1200), Utilities.Random.Next(30, 546)),
                        MathHelper.Pi,
                        15f,
                        3f,
                        75f,
                        Utilities.Random.Next(30, 50),
                        this,
                        startState: Idle.Instance());
                    SimulationWindow.EntityManager.Entities.Add(FP);
                    SimulationWindow.EntityManager.Players.Add(FP);
                    listMembers.Add(FP);
                }
            }
        }
        //Update
        public void Update(GameTime gameTime)
        {
            CalculateClosestPlayerToBall();
            teamStateMachine.Update(gameTime);
        }
        public bool CanShoot(Vector2 position, float power, Vector2 shotTarget)
        {
            //the number of randomly created shot targets this method will test 
            int NumAttempts = 5;

            while (NumAttempts-- > 0)
            {
                //choose a random position along the opponent's goal mouth. (making
                //sure the ball's radius is taken into account)
                shotTarget = OpponentsGoal.Center;

                //the y value of the shot position should lay somewhere between two
                //goalposts (taking into consideration the ball diameter)
                int MinYVal = (int)(OpponentsGoal.LeftPostPos.Y + SimulationWindow.EntityManager.Ball.Radius);
                int MaxYVal = (int)(OpponentsGoal.RightPostPos.Y - SimulationWindow.EntityManager.Ball.Radius);

                Random rand = new Random();
                shotTarget.Y = rand.Next(MinYVal, MaxYVal);
                if (isPassSafeFromAllOpponents(position, shotTarget, null, power))
                {
                    return true;
                }
            }

            return false;
        }

        private void Behaviors()
        {
            foreach (var player in SimulationWindow.EntityManager.Players)
            {
                player.Steering.StartWallAvoidance();
            }
        }

        public void ReturnAllPlayersToHome()
        {
            //TODO

        }
        public bool InControl()
        {
            if (ControllingPlayer != null)
                return true;
            return false;
        }
        public void RequestPass(FieldPlayer requester)
        {
            Random rand = new Random();
            //maybe put a restriction here
            if (rand.NextFloat(0, 1f) > 0.1f)
            {
                return;
            }
        }
        public bool isPassSafeFromAllOpponents(Vector2 from, Vector2 target, Player receiver, float PassingForce)
        {
            foreach (Player player in Opponents.listMembers)
            {
                if (!isPassSafeFromOpponent(from, target, receiver, player, PassingForce))
                {
                    return false;
                }
            }
            return true;
        }
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
        //calculate the closest player to the SupportSpot
        public Player DetermineBestSupportingAttacker() {
            float ClosestSoFar = float.MaxValue;

            Player BestPlayer = null;

            foreach (Player player in listMembers)
            {
                if ((player.PlayerRole == PlayerRole.Attacker) && (player != ControllingPlayer))
                {
                    float dist = Vector2.DistanceSquared(player.Position, SupportCalculate.GetBestSupportingSpot());
                    if (dist< ClosestSoFar)
                    {
                        ClosestSoFar = dist;
                        BestPlayer = player;
                    }
                }
            }

            return BestPlayer;
        }
        public bool CanShoot(Vector2 BallPos, float power)
        {
            return CanShoot(BallPos, power, new Vector2());
        }
        public bool FindPass(Player passer, Player receiver, Vector2 PassTarget, float power, float MinPassingDistance)
        {
            float ClosestToGoalSoFar = float.MaxValue;
            Vector2 Target = new Vector2();

            bool finded = false;
            //iterate through all this player's team members and calculate which
            //one is in a position to be passed the ball
            foreach (Player curPlayer in this.listMembers)
            {
                if ((curPlayer != passer) && (Vector2.DistanceSquared(passer.Position, curPlayer.Position)> MinPassingDistance * MinPassingDistance))
                {
                    if (GetBestPassToReceiver(passer, curPlayer, Target, power))
                    {
                        //if the pass target is the closest to the opponent's goal line found
                        // so far, keep a record of it
                        float Dist2Goal = Math.Abs(Target.X - OpponentsGoal.Center.X);

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
                float dist = Math.Abs(Passes[pass].X - OpponentsGoal.Center.X);

                if ((dist < ClosestSoFar)
                        //&& SimulationWindow.EntityManager.Field.PlayingArea.Inside(Passes[pass])
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
        private void CalculateClosestPlayerToBall()
        {
            float ClosestSoFar = float.MaxValue;

            foreach (Player curPlayer in listMembers)
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

        public Vector2 GetSupportSpot()
        {
            return SupportCalculate.GetBestSupportingSpot();
        }
    }
}

