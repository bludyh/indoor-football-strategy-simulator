﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class KickBall:State<FieldPlayer>
    {
        private static readonly KickBall instance = new KickBall();
        static KickBall(){}
        private KickBall(){}

        public static KickBall Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
        }
        public override void Handle(FieldPlayer player)
        {
        }
        public override void OnExit(FieldPlayer player)
        {
            
        }
    }
}
