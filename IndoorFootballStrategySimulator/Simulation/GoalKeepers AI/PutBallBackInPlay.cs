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
            var field = SimulationWindow.EntityManager.Field;
            var ball = SimulationWindow.EntityManager.Ball;
            Player receiver = null;
            Vector2 BallTarget = new Vector2();
            //test if there are players further forward on the field we might
            //be able to pass to. If so, make a pass.
            if (owner.Team.FindPass(owner,receiver,ref BallTarget,3f, 50f))
            {
                //make the pass   
                ball.Kick(Vector2.Normalize(BallTarget - ball.Position),3f);
                //goalkeeper no longer has ball 
                field.GoalKeeperHasBall = false;
                //let the receiving player know the ball's comin' at him
                MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MESSAGE_IMMEDIATELY,
                        owner,
                        receiver,
                        MessageTypes.Msg_ReceiveBall,
                        BallTarget);

                //go back to tending the goal   
                owner.GetFSM().ChangeState(TendGoal.Instance());
                return;
            }
            owner.Velocity = new Vector2();
        }

        public override void OnEnter(GoalKeeper owner)
        {
            //let the team know that keeper is in control
            owner.Team.SetControllingPlayer(owner);

            //send all the players home
            owner.Team.Opponent.ReturnAllPlayersToHome();
            owner.Team.ReturnAllPlayersToHome();
        }

        public override void OnExit(GoalKeeper owner)
        {
        }

        public override bool OnMessage(GoalKeeper owner, Telegram telegram)
        {
            return false;
        }
    }
}
