﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class ReturnHome : State<GoalKeeper>
    {
        private static readonly ReturnHome instance = new ReturnHome();
        static ReturnHome(){}
        private ReturnHome(){}
        public static ReturnHome Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
           // throw new NotImplementedException();
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }

        public override void OnExit(GoalKeeper owner)
        {
           // throw new NotImplementedException();
        }
    }
}
