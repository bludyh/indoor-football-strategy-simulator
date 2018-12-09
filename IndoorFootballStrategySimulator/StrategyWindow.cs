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

namespace IndoorFootballStrategySimulator {
    class StrategyWindow : UpdateWindow {

        private Field field;
        private List<Goal> goals;
        private FieldPlayer selectedPlayer;
        private TeamState teamState;

        public Strategy Strategy { get; private set; }
        public TeamState TeamState {
            get { return teamState; }
            set {
                teamState = value;
                OnTeamStateChanged();
            }
        }

        protected override void Initialize() {
            base.Initialize();

            Utilities.SimpleTexture = new Texture2D(GraphicsDevice, 1, 1);
            Utilities.SimpleTexture.SetData(new Color[] { Color.White });

            field = new Field(Editor.Content.Load<Texture2D>("SoccerField"), Color.White, new Vector2(0.75f, 0.75f), new Vector2(960f, 432f) / 2f, 0f);

            goals = new List<Goal>{
                new Goal(Editor.Content.Load<Texture2D>($"SoccerGoal"), Color.White, new Vector2(0.75f, 0.75f), new Vector2(30f, 216f), 0f),
                new Goal(Editor.Content.Load<Texture2D>($"SoccerGoal"), Color.White, new Vector2(0.75f, 0.75f), new Vector2(930, 216f), MathHelper.Pi)
            };

            Strategy = new Strategy(
                "Default Strategy",
                "Default Strategy",
                new List<Player> {
                    new GoalKeeper(Editor.Content.Load<Texture2D>($"CharacterBlue-1"), Color.White, Vector2.One, field.Areas[2].Center, 0f, 0f, 0f, 0f, 0f, homeArea: 2),
                    new FieldPlayer(Editor.Content.Load<Texture2D>($"CharacterBlue-2"), Color.White, Vector2.One, field.Areas[6].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: 6, defHomeArea: 6),
                    new FieldPlayer(Editor.Content.Load<Texture2D>($"CharacterBlue-3"), Color.White, Vector2.One, field.Areas[8].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: 8, defHomeArea: 8),
                    new FieldPlayer(Editor.Content.Load<Texture2D>($"CharacterBlue-4"), Color.White, Vector2.One, field.Areas[12].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: 12, defHomeArea: 11),
                    new FieldPlayer(Editor.Content.Load<Texture2D>($"CharacterBlue-5"), Color.White, Vector2.One, field.Areas[22].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: 22, defHomeArea: 13)
                });
        }

        private void OnTeamStateChanged() {
            foreach (var player in Strategy.Players) {
                if (player is FieldPlayer fieldPlayer)
                    fieldPlayer.Position = GetFieldPlayerHomeArea(fieldPlayer).Center;
            }
        }

        private Area GetFieldPlayerHomeArea(FieldPlayer player) {
            switch (TeamState) {
                case TeamState.OFFENSIVE:
                    return field.Areas[player.OffensiveHomeArea];
                case TeamState.DEFENSIVE:
                    return field.Areas[player.DefensiveHomeArea];
            }
            return null;
        }

        private void SetFieldPlayerHomeArea(FieldPlayer player, int area) {
            switch (TeamState) {
                case TeamState.OFFENSIVE:
                    player.OffensiveHomeArea = area;
                    break;
                case TeamState.DEFENSIVE:
                    player.DefensiveHomeArea = area;
                    break;
            }
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed) {
                var mousePos = new Vector2(mouseState.X, mouseState.Y);
                foreach (var player in Strategy.Players) {
                    if (player is FieldPlayer fieldPlayer && Vector2.Distance(mousePos, player.Position) < player.Radius && selectedPlayer == null) {
                        selectedPlayer = fieldPlayer;
                        break;
                    }
                }
                if (selectedPlayer != null)
                    selectedPlayer.Position = mousePos;
            }
            if (mouseState.LeftButton == ButtonState.Released && selectedPlayer != null) {
                for (int i = 0; i < field.Areas.Count; i++) {
                    var area = field.Areas[i];
                    if (area.Contain(selectedPlayer.Position)) {
                        if (!Strategy.Players.Any(p => p != selectedPlayer && p.Position == area.Center)) {
                            SetFieldPlayerHomeArea(selectedPlayer, i);
                            selectedPlayer.Position = area.Center;
                            break;
                        }
                        else
                            selectedPlayer.Position = GetFieldPlayerHomeArea(selectedPlayer).Center;
                    }
                }
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
            foreach (var player in Strategy.Players)
                player.Draw(Editor.spriteBatch);

            Editor.spriteBatch.End();
        }

    }
}
