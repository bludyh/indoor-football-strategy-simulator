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
        //Change to Goal object later
        private FieldPlayer homeGoal;
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
        //team color
        public enum TeamColor
        {
            Red, Blue
        }
        public List<Player> listPlayers = new List<Player>();
        private Texture2D _texture;
        private TeamColor teamColor;
        private Formation teamFormation;

        public Team(Formation formation, TeamColor color, Texture2D texture, List<Line> walls)
        {
            teamFormation = formation;
            teamColor = color;
            _texture = texture;
            CreatePlayers();
            // Avoid the Wall
            foreach (Player player in listPlayers)
            {
                player.Steering.StartWallAvoidance(walls);
                if (player is FieldPlayer)
                {
                    //player.Steering.StartSeparation(listPlayers);
                }
            }
        }
        private void CreatePlayers()
        {
            
            if (teamColor == TeamColor.Blue)
            {
                //Draw Blue Team
                //Goal Keeper
                listPlayers.Add(new GoalKeeper(_texture, Color.White, new Vector2(1f, 1f), new Vector2(140f, 372f), 0f, 100f, 3f, 75f, 50f));
                if (teamFormation == Formation.A)
                {
                    //Field Players
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 200f), 0f, 100f, 3f, 75f, 50f));
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 320f), 0f, 100f, 3f, 75f, 50f));
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 440f), 0f, 100f, 3f, 75f, 50f));
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(370f, 560f), 0f, 100f, 3f, 75f, 50f));
                }

            }
            else
            {
                //Draw Red Team
                // Goal Keeper
                listPlayers.Add(new GoalKeeper(_texture, Color.White, new Vector2(1f, 1f), new Vector2(1230f, 372f), MathHelper.Pi, 100f, 3f, 75f, 50f));
                if (teamFormation == Formation.A)
                {
                    //Field Players
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 200f), MathHelper.Pi, 100f, 3f, 75f, 50f));
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 320f), MathHelper.Pi, 100f, 3f, 75f, 50f));
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 440f), MathHelper.Pi, 100f, 3f, 75f, 50f));
                    listPlayers.Add(new FieldPlayer(Idle.Instance(), _texture, Color.White, new Vector2(1f, 1f), new Vector2(1000f, 560f), MathHelper.Pi, 100f, 3f, 75f, 50f));
                }
                
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            foreach (Player player in listPlayers)
            {
                player.Draw(spriteBatch);
            }
        }
        public void PursuitBall(Ball ball)
        {
            foreach (Player player in listPlayers)
            {
                if (player is FieldPlayer)
                {
                    player.Steering.StartPursuit(ball);
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (Player player in listPlayers)
            {
                player.Update(gameTime);
            }
        }

        public FieldPlayer HomeGoal()
        {
            return homeGoal;
        }
    }
}
