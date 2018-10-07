using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Services;

namespace IndoorFootballStrategySimulator.Game {
    /// <summary>
    ///     Manages initializing, updating and rendering process of all game objects.
    /// </summary>
    class EntityManager {

        /// <summary>
        ///     Gets a list of all entities.
        /// </summary>
        public List<Entity> Entities { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Game.Field"/> object.
        /// </summary>
        public Field Field { get; private set; }

        /// <summary>
        ///     Gets the <see cref="Game.Ball"/> object.
        /// </summary>
        public Ball Ball { get; private set; }

        /// <summary>
        ///     Gets a list of all players.
        /// </summary>
        public List<Player> Players { get; private set; }

        /// <summary>
        ///     Initializes all entities.
        /// </summary>
        /// <param name="editor"></param>
        public void Initialize(UpdateService editor) {
            Entities = new List<Entity>();

            Texture2D texture = editor.Content.Load<Texture2D>("soccerField");
            Field = new Field(texture, Color.White, new Vector2(1f, 1f), new Vector2(editor.graphics.Viewport.Width, editor.graphics.Viewport.Height) / 2f, 0f);
            Entities.Add(Field);

            texture = editor.Content.Load<Texture2D>("ball_soccer2");
            Ball = new Ball(texture, Color.White, new Vector2(1f, 1f), new Vector2(editor.graphics.Viewport.Width, editor.graphics.Viewport.Height) / 2f, 0f, 9f, 1f, 0f, 0f);
            Entities.Add(Ball);

            Players = new List<Player>();

            for (int i = 0; i < 5; i++) {
                texture = editor.Content.Load<Texture2D>("characterBlue (1)");
                var player = new Player(
                    texture, 
                    Color.White, 
                    new Vector2(1f, 1f), 
                    new Vector2(
                        MonoGameWindow.Random.Next((int)(Field.Position.X - Field.Size.X / 2f), (int)Field.Position.X), 
                        MonoGameWindow.Random.Next((int)(Field.Position.Y - Field.Size.Y / 2f), (int)(Field.Position.Y + Field.Size.Y / 2f))), 
                    0f, 
                    16f, 
                    1f, 
                    200f, 
                    MonoGameWindow.Random.Next(50, 100));
                player.Steering.StartWallAvoidance();
                player.Steering.StartSeparation();
                player.Steering.StartPursuit(Ball);
                Entities.Add(player);
                Players.Add(player);

                texture = editor.Content.Load<Texture2D>("characterRed (1)");
                player = new Player(
                    texture,
                    Color.White,
                    new Vector2(1f, 1f),
                    new Vector2(
                        MonoGameWindow.Random.Next((int)Field.Position.X, (int)(Field.Position.X + Field.Size.X / 2f)),
                        MonoGameWindow.Random.Next((int)(Field.Position.Y - Field.Size.Y / 2f), (int)(Field.Position.Y + Field.Size.Y / 2f))),
                    MathHelper.Pi,
                    16f,
                    1f,
                    200f,
                    MonoGameWindow.Random.Next(50, 100));
                player.Steering.StartWallAvoidance();
                player.Steering.StartSeparation();
                player.Steering.StartPursuit(Ball);
                Entities.Add(player);
                Players.Add(player);
            }
        }

        /// <summary>
        ///     Updates logics of all entities.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            foreach (var entity in Entities)
                entity.Update(gameTime);
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
