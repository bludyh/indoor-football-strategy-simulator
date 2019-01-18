using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class GlobalFieldPlayerState : State<FieldPlayer>
    {
        private static readonly GlobalFieldPlayerState instance = new GlobalFieldPlayerState();
        static GlobalFieldPlayerState() { }
        private GlobalFieldPlayerState() { }
        public static GlobalFieldPlayerState Instance()
        {
            return instance;
        }
        public override void Handle(FieldPlayer owner)
        {
            //if a player is in possession and close to the ball reduce his max speed
            if ((owner.BallWithinRange()) && (owner.IsControllingPlayer()))
            {
                owner.MaxSpeed = 60f;
            }
            else
            {
                owner.MaxSpeed = 80f;
            }

        }

        public override void OnEnter(FieldPlayer owner)
        {

        }

        public override void OnExit(FieldPlayer owner)
        {

        }

        public override bool OnMessage(FieldPlayer owner, Telegram telegram)
        {
            switch (telegram.Message)
            {
                case MessageTypes.Msg_ReceiveBall:
                    {
                        //set the target
                        owner.Steering.Target = (Vector2)telegram.ExtraInfo;

                        //change state 
                        owner.GetFSM().ChangeState(ReceiveBall.Instance());

                        return true;
                    }
                //break;

                case MessageTypes.Msg_SupportAttacker:
                    {
                        //if already supporting just return
                        if (owner.GetFSM().IsInState(Support.Instance()))
                        {
                            return true;
                        }

                        //set the target to be the best supporting position
                        if (owner.Team.ControllingPlayer != null)
                        {
                        owner.Steering.Target = SupportCalculate.GetBestSupportingSpot();
                        }

                        //change the state
                        owner.GetFSM().ChangeState(Support.Instance());

                        return true;
                    }

                //break;

                case MessageTypes.Msg_Wait:
                    {
                        //change the state
                        owner.GetFSM().ChangeState(Idle.Instance());

                        return true;
                    }
                // break;

                case MessageTypes.Msg_GoHome:
                    {
                        owner.GetHomeArea(SimulationWindow.EntityManager.Field,owner.Team.State);

                        owner.GetFSM().ChangeState(ReturnToHomeArea.Instance());

                        return true;
                    }

                // break;

                case MessageTypes.Msg_PassToMe:
                    {
                        //get the position of the player requesting the pass 
                        FieldPlayer receiver = (FieldPlayer)telegram.ExtraInfo;

                        //if the ball is not within kicking range or their is already a 
                        //receiving player, this player cannot pass the ball to the player
                        //making the request.
                        if (owner.Team.ReceivingPlayer != null
                                || !owner.BallWithinRange())
                        {
                            return true;
                        }

                        //make the pass   
                        SimulationWindow.EntityManager.Ball.Kick(Vector2.Subtract(receiver.Position, SimulationWindow.EntityManager.Ball.Position),3f);

                        //let the receiver know a pass is coming 
                        MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MESSAGE_IMMEDIATELY,
                                owner,
                                receiver,
                                MessageTypes.Msg_ReceiveBall,
                                receiver.Position);

                        //change state   
                        owner.GetFSM().ChangeState(Idle.Instance());

                        owner.FindSupport();

                        return true;
                    }

                    //break;

            }//end switch

            return false;
        }
    }
}
