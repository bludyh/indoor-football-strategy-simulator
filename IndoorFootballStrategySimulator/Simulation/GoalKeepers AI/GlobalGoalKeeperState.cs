using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class GlobalGoalKeeperState : State<GoalKeeper>
    {
        private static readonly GlobalGoalKeeperState instance = new GlobalGoalKeeperState();
        static GlobalGoalKeeperState() { }
        private GlobalGoalKeeperState() { }
        public static GlobalGoalKeeperState Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {

        }

        public override void OnEnter(GoalKeeper owner)
        {

        }

        public override void OnExit(GoalKeeper owner)
        {

        }

        public override bool OnMessage(GoalKeeper owner, Telegram telegram)
        {
            switch (telegram.Message)
            {
                case MessageTypes.Msg_ReceiveBall:
                    {
                        owner.GetFSM().ChangeState(InterceptBall.Instance());
                    }

                    break;
                case MessageTypes.Msg_PassToMe:
                    {
                        //get the position of the player requesting the pass 
                        FieldPlayer receiver = (FieldPlayer)telegram.ExtraInfo;

                        //if the ball is not within kicking range or there is already a 
                        //receiving player, this player cannot pass the ball to the player
                        //making the request.
                        if (owner.Team.ReceivingPlayer != null
                                || !owner.BallWithinRange())
                        {
                            return true;
                        }

                        //make the pass   
                        SimulationWindow.EntityManager.Ball.Kick(Vector2.Subtract(receiver.Position, SimulationWindow.EntityManager.Ball.Position), 3f);

                        //let the receiver know a pass is coming 
                        MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MESSAGE_IMMEDIATELY,
                                owner,
                                receiver,
                                MessageTypes.Msg_ReceiveBall,
                                receiver.Position);

                        return true;
                    }
            }
            return false;
        }
    }
}
