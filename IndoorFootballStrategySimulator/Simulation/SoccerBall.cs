using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace IndoorFootballStrategySimulator.Game
{
    class SoccerBall : MovingEntity
    {
        public Field field;
        //Keeps a record of the balls position at the last update
        public Vector2 pos;

        public double ballSize;

        //a pointer to the player (or goalkeeper) who possesses the ball
        public Player ball_owner;

        // public List<Line>
        public SoccerBall(Texture2D texture, Color color, Vector2 position, double size, float mass, float maxForce, float maxSpeed) : base(texture, color, mass, maxForce, maxSpeed)
        {
            pos = position;
            ballSize = size;
            ball_owner = null;
        }
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            pos = Velocity * deltaTime + 1 / 2;
        }
        public void Render() {

        }
        //a directional force to the ball (kicks it!)
        public void Kick(Vector2 direction, double force) {

        }

        public double TimeToCoverDistance(Vector2 from, Vector2 to, double force){
            return 1;
        }




    }
}
