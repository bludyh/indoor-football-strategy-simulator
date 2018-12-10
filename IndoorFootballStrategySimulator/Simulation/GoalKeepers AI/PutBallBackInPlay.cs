using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class PutBallBackInPlay : State<GoalKeeper>
    {
        private static readonly PutBallBackInPlay instance = new PutBallBackInPlay();
        static PutBallBackInPlay(){}
        private PutBallBackInPlay(){}
        public static PutBallBackInPlay Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
            Player receiver = null;
            Vector2 BallTarget = new Vector2();
            //TODO
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //let the team know that keeper is in control
            owner.Team.ControllingPlayer = owner;

            //send all the players home
            owner.Team.Opponents.ReturnAllPlayersToHome();
            owner.Team.ReturnAllPlayersToHome();
        }

        public override void OnExit(GoalKeeper owner)
        {
            //throw new NotImplementedException();
        }
    }
}
