using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Game {
    class EntityManager {

        private List<Entity> entities;

        public EntityManager() {
            entities = new List<Entity>();
        }

        public void Add(Entity entity) {
            entities.Add(entity);
        }

        public void Update(GameTime gameTime) {
            foreach (var entity in entities)
                entity.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }

    }
}
