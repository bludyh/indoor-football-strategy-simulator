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
        private Strategy strategy;
        private FieldPlayer selectedPlayer;

        protected override void Initialize() {
            base.Initialize();

            Utilities.SimpleTexture = new Texture2D(GraphicsDevice, 1, 1);
            Utilities.SimpleTexture.SetData(new Color[] { Color.White });

            Texture2D texture = Editor.Content.Load<Texture2D>("SoccerField");
            field = new Field(texture, Color.White, new Vector2(0.75f, 0.75f), new Vector2(960f, 432f) / 2f, 0f);

            texture = Editor.Content.Load<Texture2D>($"SoccerGoal");
            goals = new List<Goal>{
                new Goal(texture, Color.White, new Vector2(0.75f, 0.75f), new Vector2(30f, 216f), 0f),
                new Goal(texture, Color.White, new Vector2(0.75f, 0.75f), new Vector2(930, 216f), MathHelper.Pi)
            };

            texture = Editor.Content.Load<Texture2D>($"CharacterBlue-1");
            strategy = new Strategy(
                "Default Strategy",
                "Default Strategy",
                new List<Player> {
                    new GoalKeeper(texture, Color.White, new Vector2(1f, 1f), field.Areas[2].Center, 0f, 0f, 0f, 0f, 0f, homeArea: field.Areas[2]),
                    new FieldPlayer(texture, Color.White, new Vector2(1f, 1f), field.Areas[6].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: field.Areas[6]),
                    new FieldPlayer(texture, Color.White, new Vector2(1f, 1f), field.Areas[8].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: field.Areas[8]),
                    new FieldPlayer(texture, Color.White, new Vector2(1f, 1f), field.Areas[12].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: field.Areas[12]),
                    new FieldPlayer(texture, Color.White, new Vector2(1f, 1f), field.Areas[22].Center, 0f, 15f, 0f, 0f, 0f, offHomeArea: field.Areas[22])
                });
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed) {
                var mousePos = new Vector2(mouseState.X, mouseState.Y);
                foreach (var player in strategy.Players) {
                    if (player is FieldPlayer fieldPlayer && Vector2.Distance(mousePos, player.Position) < player.Radius) {
                        selectedPlayer = fieldPlayer;
                        break;
                    }
                }
                if (selectedPlayer != null)
                    selectedPlayer.Position = mousePos;
            }
            if (mouseState.LeftButton == ButtonState.Released && selectedPlayer != null) {
                foreach (var area in field.Areas) {
                    if (area.Contain(selectedPlayer.Position)) {
                        selectedPlayer.Position = area.Center;
                        break;
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
            foreach (var player in strategy.Players)
                player.Draw(Editor.spriteBatch);

            Editor.spriteBatch.End();
        }

    }
}
