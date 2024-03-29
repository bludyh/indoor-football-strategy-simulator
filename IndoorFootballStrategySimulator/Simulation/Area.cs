﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;

namespace IndoorFootballStrategySimulator.Simulation {
    public class Area {
        public enum AreaModifer
        {
            HalfSize, Normal
        }
        public float LeftX { get; private set; }
        public float RightX { get; private set; }
        public float TopY { get; private set; }
        public float BottomY { get; private set; }
        public Line Top {
            get {
                return new Line(new Vector2(LeftX, TopY), new Vector2(RightX, TopY));
            }
        }
        public Line Right {
            get {
                return new Line(new Vector2(RightX, TopY), new Vector2(RightX, BottomY));
            }
        }
        public Line Bottom {
            get {
                return new Line(new Vector2(RightX, BottomY), new Vector2(LeftX, BottomY));
            }
        }
        public Line Left {
            get {
                return new Line(new Vector2(LeftX, BottomY), new Vector2(LeftX, TopY));
            }
        }
        public float Width {
            get {
                return Math.Abs(RightX - LeftX);
            }
        }
        public float Height {
            get {
                return Math.Abs(BottomY - TopY);
            }
        }
        public Vector2 Center {
            get {
                return new Vector2((LeftX + RightX) / 2f, (TopY + BottomY) / 2f);
            }
        }
        public float Length
        {
            get { return Math.Max(Width, Height); }
        }
        public Area(float leftX, float rightX, float topY, float bottomY) {
            LeftX = leftX;
            RightX = rightX;
            TopY = topY;
            BottomY = bottomY;
        }

        public Vector2 GetRandomPosition() {
            return new Vector2(Utilities.Random.NextFloat(LeftX, RightX), Utilities.Random.NextFloat(TopY, BottomY));
        }

        public bool Contain(Vector2 pos) {
            return pos.X > LeftX && pos.X < RightX && pos.Y > TopY && pos.Y < BottomY;
        }

        public void Draw(SpriteBatch sb, Color color) {
            Top.Draw(sb, color);
            Right.Draw(sb, color);
            Bottom.Draw(sb, color);
            Left.Draw(sb, color);
        }

        public void Fill(SpriteBatch sb, Color color) {
            sb.Draw(Utilities.SimpleTexture, new Rectangle((int)LeftX, (int)TopY, (int)Width, (int)Height), color);
        }
        /* returns true if the given position lays inside the Area. 
         * The Area modifier can be used to contract the Area bounderies */
        public bool Inside(Vector2 position)
        {
            return Inside(position, AreaModifer.Normal);
        }

        public bool Inside(Vector2 position, AreaModifer areaModifer)
        {
            if (areaModifer == AreaModifer.Normal)
            {
                return ((position.X > LeftX) && (position.X < RightX)
                        && (position.Y > TopY) && (position.Y < BottomY));
            }
            else
            {
                float marginX = Width * 0.25f;
                float marginY = Height * 0.25f;

                return ((position.X > (LeftX + marginX)) && (position.X < (RightX - marginX))
                        && (position.Y > (TopY + marginY)) && (position.Y < (BottomY - marginY)));
            }
        }
    }
}
