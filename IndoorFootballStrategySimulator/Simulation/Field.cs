﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndoorFootballStrategySimulator.Simulation {
    /// <summary>
    ///     Represents a soccer field object.
    /// </summary>
    public class Field : Entity {

        private const int NumberOfColumns = 6;
        private const int NumberOfRows = 5;

        public Area PlayingArea { get; private set; }

        public List<Area> Areas { get; private set; }

        public List<Area> HomeTeamSpawnAreas { get; private set; }

        public List<Area> AwayTeamSpawnAreas { get; private set; }

        /// <summary>
        ///     Gets a list of <see cref="Line"/> that defines the border of the <see cref="Field"/>.
        /// </summary>
        public List<Line> Walls { get; private set; }
        
        public bool GoalKeeperHasBall { get;  set; }
        /// <summary>
        ///     Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="texture">texture of the field.</param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        public Field(Texture2D texture, Color color, Vector2 scale, Vector2 pos, float rot) : base(texture, color, scale, pos, rot, 0f) {
            PlayingArea = new Area(
                Size.X * 9f / 160f,
                Size.X * 151f / 160f,
                Size.Y / 72f,
                Size.Y * 71f / 72f
                );
            CreateAreas();
            HomeTeamSpawnAreas = new List<Area> { Areas[2], Areas[6], Areas[8], Areas[11], Areas[13] };
            AwayTeamSpawnAreas = new List<Area> { Areas[27], Areas[23], Areas[21], Areas[18], Areas[16] };
            Walls = new List<Line> {
                new Line(new Vector2(PlayingArea.LeftX, PlayingArea.TopY), new Vector2(PlayingArea.RightX, PlayingArea.TopY)),
                new Line(new Vector2(PlayingArea.RightX, PlayingArea.TopY), new Vector2(PlayingArea.RightX, PlayingArea.BottomY)),
                new Line(new Vector2(PlayingArea.RightX, PlayingArea.BottomY), new Vector2(PlayingArea.LeftX, PlayingArea.BottomY)),
                new Line(new Vector2(PlayingArea.LeftX, PlayingArea.BottomY), new Vector2(PlayingArea.LeftX, PlayingArea.TopY))
            };
            GoalKeeperHasBall = false;
        }

        private void CreateAreas() {
            Areas = new List<Area>();

            float width = PlayingArea.Width / NumberOfColumns;
            float height = PlayingArea.Height / NumberOfRows;
            for (int col = 0; col < NumberOfColumns; col++) {
                for (int row = 0; row < NumberOfRows; row++) {
                    Areas.Add(
                        new Area(
                            PlayingArea.LeftX + col * width,
                            PlayingArea.LeftX + (col + 1) * width,
                            PlayingArea.TopY + row * height,
                            PlayingArea.TopY + (row + 1) * height
                            ));
                }
            }
        }

        public override void Update(GameTime gameTime) { }

    }
}
