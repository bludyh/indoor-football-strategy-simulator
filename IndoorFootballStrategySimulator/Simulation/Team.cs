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
        private Player playerClosetToBall;
        private Player controllingPlayer;
        private Strategy strategy;

        public TeamColor Color { get; private set; }
        public Goal Goal { get; private set; }

        public Team Opponents { get; set; }

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
            playerClosetToBall = controllingPlayer= null; 
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

        // May delete
        private void Behaviors()
        {
            foreach (var player in Strategy.Players)
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

        public Boolean InControl() {
            if (controllingPlayer != null)
                return true;
            return false;
        }

    }
}

