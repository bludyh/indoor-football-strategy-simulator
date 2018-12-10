﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation.Team_AI
{
    public sealed class PrepareForKickOff : State<Team>
    {
        private static readonly PrepareForKickOff instance = new PrepareForKickOff();
        static PrepareForKickOff() { }
        private PrepareForKickOff() { }
        public static PrepareForKickOff Instance(){
            return instance;
        }
        public override void Handle(Team team)
        {
            throw new NotImplementedException();
        }

        public override void OnEnter(Team team)
        {
            //reset Key player pointers
            team.SetControllingPlayer(null);
            team.SetPlayerClosetToBall(null);
            //team.SetSupportingPlayer(null);
            //team.SetReceiver(null);

            team.ReturnAllPlayersToHome();
        }

        public override void OnExit(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
