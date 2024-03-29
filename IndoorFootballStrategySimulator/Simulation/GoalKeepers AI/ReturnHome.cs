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
            //if close enough to home or the opponents get control
            //over the ball and change state to tend goal
            if (!owner.Team.InControl() && owner.InHomeArea()) {
                owner.GetFSM().ChangeState(TendGoal.Instance());
            }
        }

        public override void OnEnter(GoalKeeper owner)
        {
            var field = SimulationWindow.EntityManager.Field;
            owner.Steering.Target = owner.GetHomeArea(field, owner.Team.State).Center;
            owner.Steering.StartArrival(owner.Steering.Target);
        }

        public override void OnExit(GoalKeeper owner)
        {
            owner.Steering.StopArrival();
        }

        public override bool OnMessage(GoalKeeper owner, Telegram telegram)
        {
            return false;
        }
    }
}
