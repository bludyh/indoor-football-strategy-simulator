﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Game;

namespace IndoorFootballStrategySimulator {
    class MonoGameWindow : UpdateWindow {

        public static Texture2D lineTexture;

        private float frameRate;
        private Field field;
        private Ball ball;
        private Team blueteam, redteam;

        protected override void Initialize() {
            base.Initialize();

            lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture.SetData(new Color[] { Color.White });
            //Soccer Field  
            Texture2D texture = Editor.Content.Load<Texture2D>("soccerField");
            field = new Field(texture, Color.White, new Vector2(1f, 1f), new Vector2(Editor.graphics.Viewport.Width / 2f, Editor.graphics.Viewport.Height / 2f), 0f);
            // Soccer Ball
            texture = Editor.Content.Load<Texture2D>("soccerBall");
            ball = new Ball(texture, Color.White, new Vector2(1f, 1f), new Vector2(685f, 372f), 1f, 0f, 0f);
            //Team Blue
            
            texture = Editor.Content.Load<Texture2D>("characterBlue (1)");
            blueteam = new Team(Team.Formation.A, Team.TeamColor.Blue, texture);
            //GKBlue = new GoalKeeper(texture, Color.White, new Vector2(1f, 1f), new Vector2(120f, 372f), 0f, 0f, 3f, 1000f, 100f);
            //playerBlue = new FieldPlayer(texture, Color.White, new Vector2(1f, 1f), new Vector2(300f, 300f), 0f, 0f, 3f, 1000f, 100f);
            // Team Red
            texture = Editor.Content.Load<Texture2D>("characterRed (1)");
            redteam = new Team(Team.Formation.A, Team.TeamColor.Red, texture);
            //playerRed = new FieldPlayer(texture, Color.White, new Vector2(1f, 1f), new Vector2(700f, 500f), MathHelper.Pi, 0f, 3f, 200f, 50f);
            ////

            ////playerBlue.Steering.StartWallAvoidance(field.Walls);
            //playerRed.Steering.StartSeek();
            //playerRed.Steering.StartWallAvoidance(field.Walls);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            //playerBlue.Steering.StartArrival(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            //playerBlue.Update(gameTime);
            //playerRed.Update(gameTime);
            //ball
            ball.Update(gameTime);
        }

        protected override void Draw() {
            base.Draw();

            Editor.spriteBatch.Begin();
            Editor.spriteBatch.DrawString(Editor.Font, $"fps: { frameRate.ToString("0.0") }", new Vector2(10f, 10f), Color.White);
            //Draw Field
            field.Draw(Editor.spriteBatch);
            //Draw Team
            //GKBlue.Draw(Editor.spriteBatch);
            //playerBlue.Draw(Editor.spriteBatch);
            //playerRed.Draw(Editor.spriteBatch);
            blueteam.Draw(Editor.spriteBatch);
            redteam.Draw(Editor.spriteBatch);
            //Draw Ball
            ball.Draw(Editor.spriteBatch);

            Editor.spriteBatch.End();
        }

        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color) {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(lineTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

    }
}
