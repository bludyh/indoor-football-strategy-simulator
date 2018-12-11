using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Services;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Manages initializing, updating and rendering process of all game objects.
    /// </summary>
    public class EntityManager {

        /// <summary>
        ///     Gets a list of all entities.
        /// </summary>
        public List<Entity> Entities { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Simulation.Field"/> object.
        /// </summary>
        public Field Field { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Simulation.Ball"/> object.
        /// </summary>
        public Ball Ball { get; private set; }

        public Team BlueTeam { get; private set; }
        public Team RedTeam { get; private set; }

        public EntityManager() {
            Entities = new List<Entity>();
        }

        /// <summary>
        ///     Initializes all entities.
        /// </summary>
        /// <param name="editor"></param>
        public void Initialize(UpdateService editor) {
            Field = new Field(editor.Content.Load<Texture2D>("SoccerField"), Color.White, new Vector2(1f), new Vector2(640f, 288f), 0f);
            Entities.Add(Field);

            Ball = new Ball(editor.Content.Load<Texture2D>("SoccerBall"), Color.White, new Vector2(1f), new Vector2(640f, 288f), 0f, 9f, 1f, 0f, 0f);
            Entities.Add(Ball);

            BlueTeam = new Team(editor, TeamColor.BLUE);
            RedTeam = new Team(editor, TeamColor.RED);
        }

        /// <summary>
        ///     Updates logics of all entities.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            foreach (var entity in Entities)
                entity.Update(gameTime);

            BlueTeam.Update(gameTime);
            RedTeam.Update(gameTime);
        }

        /// <summary>
        ///     Draws all entities on the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            foreach (var entity in Entities)
                entity.Draw(spriteBatch);
        }
        
    }
}
