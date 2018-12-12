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

        private TeamColor Team {
            get {
                switch (Name) {
                    case "strategyPreviewWindowHome":
                        return TeamColor.BLUE;
                    case "strategyPreviewWindowAway":
                        return TeamColor.RED;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

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
                var p = Strategy.Players[i];
                if (p is GoalKeeper goalKeeper) {
                    var player = new GoalKeeper(
                        Editor.Content.Load<Texture2D>($"Character{ ((Team == TeamColor.BLUE) ? "Blue" : "Red") }-{ i + 1 }"),
                        Color.White,
                        Vector2.One,
                        field.Areas[(Team == TeamColor.BLUE) ? goalKeeper.HomeArea : 29 - goalKeeper.HomeArea].Center,
                        (Team == TeamColor.BLUE) ? 0f : MathHelper.Pi,
                        15f,
                        3f,
                        75f,
                        50f,
                        Team,
                        (Team == TeamColor.BLUE) ? goalKeeper.HomeArea : 29 - goalKeeper.HomeArea,
                        (Team == TeamColor.BLUE) ? goalKeeper.Areas : goalKeeper.Areas.Select(a => 29 - a).ToList(),
                        TendGoal.Instance());
                    players.Add(player);
                }
                else if (p is FieldPlayer fieldPlayer) {
                    var player = new FieldPlayer(
                        Editor.Content.Load<Texture2D>($"Character{ ((Team == TeamColor.BLUE) ? "Blue" : "Red") }-{ i + 1 }"),
                        Color.White,
                        Vector2.One,
                        field.Areas[(Team == TeamColor.BLUE) ? fieldPlayer.OffensiveHomeArea : 29 - fieldPlayer.OffensiveHomeArea].Center,
                        (Team == TeamColor.BLUE) ? 0f : MathHelper.Pi,
                        15f,
                        3f,
                        75f,
                        50f,
                        Team,
                        (Team == TeamColor.BLUE) ? fieldPlayer.OffensiveHomeArea : 29 - fieldPlayer.OffensiveHomeArea,
                        (Team == TeamColor.BLUE) ? fieldPlayer.OffensiveAreas : fieldPlayer.OffensiveAreas.Select(a => 29 - a).ToList(),
                        (Team == TeamColor.BLUE) ? fieldPlayer.DefensiveHomeArea : 29 - fieldPlayer.DefensiveHomeArea,
                        (Team == TeamColor.BLUE) ? fieldPlayer.DefensiveAreas : fieldPlayer.DefensiveAreas.Select(a => 29 - a).ToList(),
                        ChaseBall.Instance());
                    players.Add(player);
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
