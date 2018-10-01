using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game
{
    class GoalKeeper : Player
    {
        public GoalKeeper(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot, float radius, float mass, float maxForce, float maxSpeed)
            : base(texture, color, scale, pos, rot, radius, mass, maxForce, maxSpeed)
        {
            Scale = scale;
            Position = pos;
            Rotation = rot;
            Radius = radius;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
