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
    class StrategyPreviewWindow : StrategyWindow {

        protected override void Initialize() {
            base.Initialize();

            field = new Field(Editor.Content.Load<Texture2D>("SoccerField"), Color.White, new Vector2(0.5f), new Vector2(640, 288) / 2f, 0f);

            goals = new List<Goal>{
                new Goal(Editor.Content.Load<Texture2D>($"SoccerGoal"), Color.White, new Vector2(0.5f), new Vector2(20f, 144), 0f),
                new Goal(Editor.Content.Load<Texture2D>($"SoccerGoal"), Color.White, new Vector2(0.5f), new Vector2(620, 144f), MathHelper.Pi)
            };
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
                        new Vector2(0.5f),
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
                        new Vector2(0.5f),
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

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();

            field.Draw(Editor.spriteBatch);
            foreach (var goal in goals)
                goal.Draw(Editor.spriteBatch);
            if (Strategy != null) {
                foreach (var player in Strategy.Players)
                    player.Draw(Editor.spriteBatch);
            }

            Editor.spriteBatch.End();
        }

    }
}
