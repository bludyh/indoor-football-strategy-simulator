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
    public class PutBallBackInPlay : State<GoalKeeper>
    {
        private static PutBallBackInPlay instance = new PutBallBackInPlay();
        public static PutBallBackInPlay Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {
            Player receiver = null;
            Vector2 BallTarget = new Vector2();
            
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //let the team know that keeper is in control
            owner.Team.SetControllingPlayer(owner);

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
