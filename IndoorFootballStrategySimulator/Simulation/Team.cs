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

        public Team(TeamColor color, UpdateService editor)
        {
            teamColor = color;
            CreatePlayers(editor);
            Behaviors();
        }
        private void CreatePlayers(UpdateService editor)
        {
            
            if (teamColor == TeamColor.Blue)
            {
                //Draw Blue Team
                Texture2D texture = editor.Content.Load<Texture2D>($"CharacterBlue-{ Simulator.Random.Next(1, 6) }");
                //Goal Keeper
                GoalKeeper GK = new GoalKeeper(this, 
                    TendGoal.Instance(), 
                    texture, 
                    Color.White, 
                    new Vector2(1f, 1f), 
                    new Vector2(80f, 288f), 
                    0f, 
                    15f, 
                    3f, 
                    75f, 
                    50f);
                SimulationWindow.EntityManager.Entities.Add(GK);
                SimulationWindow.EntityManager.Players.Add(GK);
                //Field Players
                for (int i = 0; i < 4; i++)
                {
                    texture = editor.Content.Load<Texture2D>($"CharacterBlue-{ Simulator.Random.Next(1, 6) }");
                    FieldPlayer FP = new FieldPlayer(this,
                        Idle.Instance(), 
                        texture, 
                        Color.White, 
                        new Vector2(1f, 1f),
                        new Vector2(Simulator.Random.Next(80, 640), Simulator.Random.Next(30, 546)),
                        0f, 
                        15f, 
                        3f,
                        75f,
                        Simulator.Random.Next(30, 50));
                    SimulationWindow.EntityManager.Entities.Add(FP);
                    SimulationWindow.EntityManager.Players.Add(FP);
                }
            }
            else
            {
                //Draw Red Team
                Texture2D texture = editor.Content.Load<Texture2D>($"CharacterRed-{ Simulator.Random.Next(1, 6) }");
                // Goal Keeper
                GoalKeeper GK = new GoalKeeper(this,
                    TendGoal.Instance(), 
                    texture, 
                    Color.White, 
                    new Vector2(1f, 1f), 
                    new Vector2(1200f, 288f), 
                    MathHelper.Pi, 
                    15f, 
                    3f, 
                    75f, 
                    50f);
                SimulationWindow.EntityManager.Entities.Add(GK);
                SimulationWindow.EntityManager.Players.Add(GK);
                //Field Players
                for (int i = 0; i < 4; i++)
                {
                    texture = editor.Content.Load<Texture2D>($"CharacterRed-{ Simulator.Random.Next(1, 6) }");
                    FieldPlayer FP = new FieldPlayer(this,
                        Idle.Instance(), 
                        texture, 
                        Color.White,
                        new Vector2(1f, 1f),
                        new Vector2(Simulator.Random.Next(640, 1200), Simulator.Random.Next(30, 546)),
                        MathHelper.Pi, 
                        15f, 
                        3f, 
                        75f,
                        Simulator.Random.Next(30, 50));
                    SimulationWindow.EntityManager.Entities.Add(FP);
                    SimulationWindow.EntityManager.Players.Add(FP);
                }
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
    }
}

