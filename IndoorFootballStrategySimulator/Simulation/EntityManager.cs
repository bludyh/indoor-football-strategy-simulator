﻿using System;
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

        /// <summary>
        ///     Gets a list of all players.
        /// </summary>
        public List<Player> Players { get; private set; }

        private Team BlueTeam;
        private Team RedTeam;

        /// <summary>
        ///     Initializes all entities.
        /// </summary>
        /// <param name="editor"></param>
        public void Initialize(UpdateService editor) {
            Entities = new List<Entity>();

            Texture2D texture = editor.Content.Load<Texture2D>("SoccerField");
            Field = new Field(texture, Color.White, new Vector2(1f, 1f), new Vector2(1280f, 576f) / 2f, 0f);
            Entities.Add(Field);

            texture = editor.Content.Load<Texture2D>("SoccerBall");
            Ball = new Ball(texture, Color.White, new Vector2(1f, 1f), new Vector2(1280f, 576f) / 2f, 0f, 9f, 1f, 0f, 0f);
            Entities.Add(Ball);

            Players = new List<Player>();
            BlueTeam = new Team(Team.TeamColor.Blue, editor);
            RedTeam = new Team(Team.TeamColor.Red, editor);
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