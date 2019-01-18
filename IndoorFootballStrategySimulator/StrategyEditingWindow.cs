using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Simulation;
using System.Windows.Forms;

namespace IndoorFootballStrategySimulator {
    class StrategyEditingWindow : StrategyWindow {

        private Player selectedPlayer;
        private bool isPlayerBeingDragged;

        public override Strategy Strategy {
            get { return strategy; }
            protected set => SetStrategy(value);
        }

        protected override void Initialize() {
            base.Initialize();

            field = new Field(Editor.Content.Load<Texture2D>("SoccerField"), Color.White, new Vector2(0.75f), new Vector2(960f, 432f) / 2f, 0f);

            goals = new List<Goal>{
                new Goal(Editor.Content.Load<Texture2D>($"SoccerGoal"), Color.White, new Vector2(0.75f), new Vector2(30f, 216f), 0f),
                new Goal(Editor.Content.Load<Texture2D>($"SoccerGoal"), Color.White, new Vector2(0.75f), new Vector2(930, 216f), MathHelper.Pi)
            };
        }

        private void SetStrategy(Strategy strategy) {
            this.strategy = strategy;
            selectedPlayer = null;
        }

        private void SetPlayerHomeArea(Player player, int area) {
            if (player is FieldPlayer fieldPlayer) {
                switch (TeamState) {
                    case TeamState.OFFENSIVE:
                        fieldPlayer.OffensiveHomeArea = area;
                        fieldPlayer.OffensiveAreas.Clear();
                        fieldPlayer.OffensiveAreas.Add(area);
                        break;
                    case TeamState.DEFENSIVE:
                        fieldPlayer.DefensiveHomeArea = area;
                        fieldPlayer.DefensiveAreas.Clear();
                        fieldPlayer.DefensiveAreas.Add(area);
                        break;
                }
            }
        }

        private void SetPlayerAreas(Player player, int area) {
            if (player is  FieldPlayer fieldPlayer) {
                switch (TeamState) {
                    case TeamState.OFFENSIVE:
                        if (area != fieldPlayer.OffensiveHomeArea) {
                            if (fieldPlayer.OffensiveAreas.Contains(area))
                                fieldPlayer.OffensiveAreas.Remove(area);
                            else
                                fieldPlayer.OffensiveAreas.Add(area);
                        }
                        break;
                    case TeamState.DEFENSIVE:
                        if (area != fieldPlayer.DefensiveHomeArea) {
                            if (fieldPlayer.DefensiveAreas.Contains(area))
                                fieldPlayer.DefensiveAreas.Remove(area);
                            else
                                fieldPlayer.DefensiveAreas.Add(area);
                        }
                        break;
                }
            }
        }

        public void CreateNewStrategy() {
            Strategy = new Strategy(
                "New Strategy",
                "New Strategy",
                new List<Player> {
                    new GoalKeeper(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-1"),
                        Color.White,
                        Vector2.One,
                        field.Areas[2].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        homeArea: 2,
                        areas: new List<int> { 1, 2, 3 } ),
                    new FieldPlayer(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-2"),
                        Color.White,
                        Vector2.One,
                        field.Areas[6].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        offHomeArea: 6,
                        offAreas: new List<int> { 6 },
                        defHomeArea: 6,
                        defAreas: new List<int> { 6 } ),
                    new FieldPlayer(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-3"),
                        Color.White,
                        Vector2.One,
                        field.Areas[8].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        offHomeArea: 8,
                        offAreas: new List<int> { 8 },
                        defHomeArea: 8,
                        defAreas: new List<int> { 8 } ),
                    new FieldPlayer(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-4"),
                        Color.White,
                        Vector2.One,
                        field.Areas[12].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        offHomeArea: 12,
                        offAreas: new List<int> { 12 },
                        defHomeArea: 11,
                        defAreas: new List<int> { 11 } ),
                    new FieldPlayer(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-5"),
                        Color.White,
                        Vector2.One,
                        field.Areas[22].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        offHomeArea: 22,
                        offAreas: new List<int> { 22 },
                        defHomeArea: 13,
                        defAreas: new List<int> { 13 } )
                });
        }

        public void SaveStrategyToFile(string fileName) {
            Utilities.Serialize(Strategy, fileName);
        }

        public override void LoadStrategyFromFile(string fileName) {
            Strategy = Utilities.Deserialize<Strategy>(fileName);

            var players = new List<Player>();

            for (int i = 0; i < Strategy.Players.Count; i++) {
                var player = Strategy.Players[i];
                if (player is GoalKeeper goalKeeper) {
                    players.Add(new GoalKeeper(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-{ i + 1 }"),
                        Color.White,
                        Vector2.One,
                        field.Areas[goalKeeper.HomeArea].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        homeArea: goalKeeper.HomeArea,
                        areas: goalKeeper.Areas
                        ));
                }
                else if (player is FieldPlayer fieldPlayer) {
                    players.Add(new FieldPlayer(
                        Editor.Content.Load<Texture2D>($"CharacterBlue-{ i + 1 }"),
                        Color.White,
                        Vector2.One,
                        field.Areas[fieldPlayer.OffensiveHomeArea].Center,
                        0f,
                        15f,
                        0f,
                        0f,
                        0f,
                        offHomeArea: fieldPlayer.OffensiveHomeArea,
                        offAreas: fieldPlayer.OffensiveAreas,
                        defHomeArea: fieldPlayer.DefensiveHomeArea,
                        defAreas: fieldPlayer.DefensiveAreas
                        ));
                }
            }
            Strategy.Players = players;
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);

            var mousePos = new Vector2(e.Location.X, e.Location.Y);

            if (e.Button == MouseButtons.Left) {
                foreach (var player in Strategy.Players) {
                    if (Vector2.Distance(mousePos, player.Position) < player.Radius) {
                        if (selectedPlayer == null) {
                            selectedPlayer = player;
                            selectedPlayer.Scale = new Vector2(1.2f);
                        }
                        if (selectedPlayer is FieldPlayer && player == selectedPlayer)
                            isPlayerBeingDragged = true;
                        break;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            var mousePos = new Vector2(e.Location.X, e.Location.Y);

            if (selectedPlayer != null && isPlayerBeingDragged)
                selectedPlayer.Position = mousePos;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);

            var mousePos = new Vector2(e.Location.X, e.Location.Y);

            if (e.Button == MouseButtons.Left && selectedPlayer != null && selectedPlayer is FieldPlayer) {
                for (int i = 0; i < field.Areas.Count; i++) {
                    var area = field.Areas[i];

                    if (area.Inside(mousePos)) {
                        if (isPlayerBeingDragged) {
                            if (area != selectedPlayer.GetHomeArea(field, teamState) && !Strategy.Players.Any(p => p != selectedPlayer && p.Position == area.Center)) {
                                SetPlayerHomeArea(selectedPlayer, i);
                                selectedPlayer.Position = area.Center;
                            }
                            else
                                selectedPlayer.Position = selectedPlayer.GetHomeArea(field, teamState).Center;
                        }
                        else
                            SetPlayerAreas(selectedPlayer, i);
                        break;
                    }
                }
                isPlayerBeingDragged = false;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Right && selectedPlayer != null) {
                selectedPlayer.Scale = Vector2.One;
                selectedPlayer = null;
            }
        }

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();

            field.Draw(Editor.spriteBatch);
            foreach (var area in field.Areas)
                area.Draw(Editor.spriteBatch, Color.Red);
            foreach (var goal in goals)
                goal.Draw(Editor.spriteBatch);
            if (Strategy != null) {
                foreach (var player in Strategy.Players)
                    player.Draw(Editor.spriteBatch);
            }
            if (selectedPlayer != null) {
                foreach (var area in selectedPlayer.GetAreas(field, teamState))
                    area.Fill(Editor.spriteBatch, Color.Red * 0.2f);
            }

            Editor.spriteBatch.End();
        }

    }
}
