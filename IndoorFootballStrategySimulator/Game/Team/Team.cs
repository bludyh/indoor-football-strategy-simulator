using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Game
{
    class Team
    {
        //Formation
        public enum Formation
        {
            A = 400,
            B = 310,
            C = 220,
            D = 211,
            E = 130,
            F = 121,
            G = 112
        }
        public enum TeamColor
        {
            Red, Blue
        }
        private GoalKeeper GK;
        private FieldPlayer FP1,FP2,FP3,FP4;
        private Texture2D _texture;
        private TeamColor teamColor;
        private Formation teamFormation;

        public Team(Formation formation, TeamColor color, Texture2D texture)
        {
            teamFormation = formation;
            teamColor = color;
            _texture = texture;
            CreatePlayers();
        }
        public void CreatePlayers()
        {
            
            if (teamColor == TeamColor.Blue)
            {
                //Draw Blue Team
                //goalkeeper
                GK = new GoalKeeper(_texture, Color.White, new Vector2(1f, 1f), new Vector2(140f, 372f), 0f, 0f, 3f, 1000f, 100f);
                if (teamFormation == Formation.A)
                {
                    //fieldplayer
                    FP1 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 200f), 0f, 0f, 3f, 1000f, 100f);
                    FP2 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 320f), 0f, 0f, 3f, 1000f, 100f);
                    FP3 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 440f), 0f, 0f, 3f, 1000f, 100f);
                    FP4 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 560f), 0f, 0f, 3f, 1000f, 100f);
                }

            }
            else
            {
                //Draw Red Team
                //goalkeeper
                GK = new GoalKeeper(_texture, Color.White, new Vector2(1f, 1f), new Vector2(1230f, 372f), MathHelper.Pi, 0f, 3f, 1000f, 100f);
                if (teamFormation == Formation.A)
                {
                    //fieldplayer
                    FP1 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 200f), MathHelper.Pi, 0f, 3f, 1000f, 100f);
                    FP2 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 320f), MathHelper.Pi, 0f, 3f, 1000f, 100f);
                    FP3 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 440f), MathHelper.Pi, 0f, 3f, 1000f, 100f);
                    FP4 = new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 560f), MathHelper.Pi, 0f, 3f, 1000f, 100f);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            GK.Draw(spriteBatch);
            FP1.Draw(spriteBatch);
            FP2.Draw(spriteBatch);
            FP3.Draw(spriteBatch);
            FP4.Draw(spriteBatch);
        }
    }
}
