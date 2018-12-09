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
        public enum TeamColor
        {
            Red, Blue
        }
        public List<Player> Players = new List<Player>();
        private TeamColor teamColor;
        private Player playerClosetToBall;
        private Player controllingPlayer;

        public Team Opponents { get; set; }
        private readonly FSM<Team> teamStateMachine;

        public Goal Goal { get; private set; }
        public Goal HomeGoal { get; private set; }

        public Team(TeamColor color, UpdateService editor)
        {
            teamColor = color;
            CreatePlayers(editor);
            Behaviors();
            teamStateMachine = new FSM<Team>(this);
            teamStateMachine.SetCurrentState(Defensive.Instance());
            playerClosetToBall = controllingPlayer= null; 
        }
        private void CreatePlayers(UpdateService editor)
        {
            
            if (teamColor == TeamColor.Blue)
            {
                //Draw Blue Team
                Texture2D texture = editor.Content.Load<Texture2D>($"CharacterBlue-{ Utilities.Random.Next(1, 6) }");
                //Goal Keeper
                GoalKeeper GK = new GoalKeeper(
                    texture,
                    Color.White,
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
                //Field Players
                for (int i = 0; i < 4; i++)
                {
                    texture = editor.Content.Load<Texture2D>($"CharacterBlue-{ Utilities.Random.Next(1, 6) }");
                    FieldPlayer FP = new FieldPlayer(
                        texture,
                        Color.White,
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
                }

                texture = editor.Content.Load<Texture2D>($"SoccerGoal");
                Goal = new Goal(texture, Color.White, new Vector2(1f, 1f), new Vector2(40f, 288f), 0f);
                SimulationWindow.EntityManager.Entities.Add(Goal);
            }
            else
            {
                //Draw Red Team
                Texture2D texture = editor.Content.Load<Texture2D>($"CharacterRed-{ Utilities.Random.Next(1, 6) }");
                // Goal Keeper
                GoalKeeper GK = new GoalKeeper(
                    texture,
                    Color.White,
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
                //Field Players
                for (int i = 0; i < 4; i++)
                {
                    texture = editor.Content.Load<Texture2D>($"CharacterRed-{ Utilities.Random.Next(1, 6) }");
                    FieldPlayer FP = new FieldPlayer(
                        texture,
                        Color.White,
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
                }

                texture = editor.Content.Load<Texture2D>($"SoccerGoal");
                Goal = new Goal(texture, Color.White, new Vector2(1f, 1f), new Vector2(1240f, 288f), MathHelper.Pi);
                SimulationWindow.EntityManager.Entities.Add(Goal);
            }
        }
        private void Behaviors()
        {
            foreach (var player in SimulationWindow.EntityManager.Players)
            {
                player.Steering.StartWallAvoidance();
                if (player is FieldPlayer)
                {
                    player.Steering.StartPursuit(SimulationWindow.EntityManager.Ball);
                }
            }
        }
        public void SetPlayerClosetToBall(Player player) {
            playerClosetToBall = player;
        }
        public Player PlayerClosetToBall() {
            return playerClosetToBall;
        }
        public void SetControllingPlayer(Player player) {
            controllingPlayer = player;
        }
        public void ReturnAllPlayersToHome() {
            //TODO
           
        }
        public bool InControl() {
            if (controllingPlayer != null)
                return true;
            return false;
        }
    }
}

