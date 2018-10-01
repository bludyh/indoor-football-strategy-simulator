using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Game
{
    class Ball:MovingEntity
    {
        public Ball(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float mass, float maxForce, float maxSpeed)
            :base(texture,color, mass, maxForce,maxSpeed)
        {
            Scale = scale;
            Position = pos;
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
